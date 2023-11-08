using tapr_2023_equipe1_carro_dotnet.Models;

namespace tapr_2023_equipe1_carro_dotnet.Services;

public interface ICarroService
{
    Task<List<Carro>> GetAllAsync();
    Task<Carro> GetByIdAsync(string id);
    Task<Carro> saveNewAsync(Carro carro);
    Task<Carro> updateAsync(String id, Carro carro);
    Task<Carro> DeleteAsync(String id);
}