using Microsoft.AspNetCore.Mvc;
using tapr_2023_equipe1_carro_dotnet.Models;

namespace tapr_2023_equipe1_carro_dotnet.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class CarroController : ControllerBase
{

    [HttpGet]
    public async Task<List<Carro>> Get(){
        var listaCarros = new List<Carro>();

        return listaCarros;
    }
}