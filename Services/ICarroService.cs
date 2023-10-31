using tapr_2023_equipe1_carro_dotnet.Models;

namespace tapr_2023_equipe1_carro_dotnet.Services;

public interface ICarroService
{
    Task<List<Carro>> GetAllAsync();
}