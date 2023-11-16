using tapr_2023_equipe1_carro_dotnet.Models;

namespace tapr_2023_equipe1_carro_dotnet.Services;

public interface IClienteService
{
    Task<List<Cliente>> GetAllAsync();
    Task<Cliente> GetByIdAsync(string id);
    Task<Cliente> saveNewAsync(Cliente cliente);
    Task<Cliente> updateAsync(String id, Cliente cliente);
    Task<Cliente> DeleteAsync(String id);
}