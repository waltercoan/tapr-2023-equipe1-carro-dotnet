using Microsoft.AspNetCore.Mvc;
using tapr_2023_equipe1_carro_dotnet.Models;
using tapr_2023_equipe1_carro_dotnet.Services;
using Dapr;


namespace tapr_2023_equipe1_carro_dotnet.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class ClienteController : ControllerBase
{
    private IClienteService _service;
    private IConfiguration _configuration;
    public ClienteController(IClienteService service)
    {
        this._service = service;
    }

    [HttpGet]
    public async Task<IResult> Get(){
        var listaClientes = await _service.GetAllAsync();
        
        return Results.Ok(listaClientes);
    }
    [HttpGet("{id}")]
    public async Task<IResult> GetById(String id){
        var listaClientes = await _service.GetByIdAsync(id);
        if(listaClientes == null){
            return Results.NotFound();
        }
        return Results.Ok(listaClientes);
    }
    [HttpPost()]
    public async Task<IResult> InsertNew(Cliente Cliente){      
        if(Cliente == null){
            return Results.BadRequest();
        }
        await _service.saveNewAsync(Cliente);

        return Results.Ok(Cliente);
    }
    [HttpPut("{id}")]
    public async Task<IResult> Update(string id, Cliente Cliente){      
        if(Cliente == null || id.Equals(String.Empty))
        {
            return Results.BadRequest();
        }

        Cliente = await _service.updateAsync(id, Cliente);

        if(Cliente == null)
        { 
            return Results.NotFound();
        }

        return Results.Ok(Cliente);
    }
    [HttpDelete("{id}")]
    public async Task<IResult> Delete(string id){      
        if(id.Equals(String.Empty))
        {
            return Results.BadRequest();
        }

        var Cliente = await _service.DeleteAsync(id);

        if(Cliente == null)
        { 
            return Results.NotFound();
        }

        return Results.Ok(Cliente);
    }
    
    [Topic(pubsubName:"servicebus-pubsub",name:"topico-equipe-0-cliente")] 
    [HttpPost("/event")]
    public async Task<IResult> UpdateClient(Cliente Cliente){      
        if(Cliente == null){
            return Results.BadRequest();
        }
        Console.WriteLine("EVENT" + Cliente.Nome);
        await _service.updateEventAsync(Cliente);

        return Results.Ok(Cliente);
    }
}