using Microsoft.AspNetCore.Mvc;
using tapr_2023_equipe1_carro_dotnet.Models;
using tapr_2023_equipe1_carro_dotnet.Services;

namespace tapr_2023_equipe1_carro_dotnet.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class CarroController : ControllerBase
{
    private ICarroService _service;
    public CarroController(ICarroService service)
    {
        this._service = service;
    }

    [HttpGet]
    public async Task<IResult> Get(){
        var listaCarros = await _service.GetAllAsync();
        
        return Results.Ok(listaCarros);
    }
    [HttpGet("{id}")]
    public async Task<IResult> GetById(String id){
        var listaCarros = await _service.GetByIdAsync(id);
        if(listaCarros == null){
            return Results.NotFound();
        }
        return Results.Ok(listaCarros);
    }
    [HttpPost()]
    public async Task<IResult> InsertNew(Carro carro){      
        if(carro == null){
            return Results.BadRequest();
        }
        await _service.saveNewAsync(carro);

        return Results.Ok(carro);
    }
    [HttpPut("{id}")]
    public async Task<IResult> Update(string id, Carro carro){      
        if(carro == null || id.Equals(String.Empty))
        {
            return Results.BadRequest();
        }

        carro = await _service.updateAsync(id, carro);

        if(carro == null)
        { 
            return Results.NotFound();
        }

        return Results.Ok(carro);
    }
    [HttpDelete("{id}")]
    public async Task<IResult> Delete(string id){      
        if(id.Equals(String.Empty))
        {
            return Results.BadRequest();
        }

        var carro = await _service.DeleteAsync(id);

        if(carro == null)
        { 
            return Results.NotFound();
        }

        return Results.Ok(carro);
    }
}