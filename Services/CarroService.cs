using Microsoft.EntityFrameworkCore;
using tapr_2023_equipe1_carro_dotnet.Models;

namespace tapr_2023_equipe1_carro_dotnet.Services;

public class CarroService : ICarroService
{
    private RepositoryDbContext _dbContext;
    public CarroService(RepositoryDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    public async Task<List<Carro>> GetAllAsync()
    {
        var listaCarros = await _dbContext.Carros.ToListAsync();
        return listaCarros;
    }
}