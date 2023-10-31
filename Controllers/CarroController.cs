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
    public async Task<List<Carro>> Get(){
        //var listaCarros = new List<Carro>();
        var listaCarros = await _service.GetAllAsync();

        return listaCarros;
    }
}