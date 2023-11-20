using Dapr.Client;
using Microsoft.EntityFrameworkCore;
using tapr_2023_equipe1_carro_dotnet.Models;

namespace tapr_2023_equipe1_carro_dotnet.Services;

public class CarroService : ICarroService
{
    private RepositoryDbContext _dbContext;
    private IConfiguration _configuration;
    private DaprClient _daprClient;
    public CarroService(RepositoryDbContext dbContext,
                        IConfiguration configuration)
    {
        this._dbContext = dbContext;
        this._configuration = configuration;
        this._daprClient = new DaprClientBuilder().Build();
        
    }

    public async Task<List<Carro>> GetAllAsync()
    {
        var listaCarros = await _dbContext.Carros.ToListAsync();
        return listaCarros;
    }

    public async Task<Carro> GetByIdAsync(string id)
    {
        var carro = await _dbContext.Carros.Where(c => c.id.Equals(new Guid(id))).FirstOrDefaultAsync();
        return carro;
        
    }

    public async Task<Carro> saveNewAsync(Carro carro)
    {
        carro.id = Guid.Empty;
        await _dbContext.Carros.AddAsync(carro);
        await _dbContext.SaveChangesAsync();
        await PublishUpdateAsync(carro);
        return carro;
    }

    public async Task<Carro> updateAsync(string id, Carro carro)
    {
        var carroAntigo = await _dbContext.Carros.Where(c => c.id.Equals(new Guid(id))).FirstOrDefaultAsync();        
        if (carroAntigo != null){
            //Atualizar cada atributo do objeto antigo 
            carroAntigo.modelo = carro.modelo;
            await _dbContext.SaveChangesAsync();
            await PublishUpdateAsync(carroAntigo);
        }
        return carroAntigo;
    }
    public async Task<Carro> DeleteAsync(string id)
    {
        var carroAntigo = await _dbContext.Carros.Where(c => c.id.Equals(new Guid(id))).FirstOrDefaultAsync();
        if (carroAntigo != null){
             _dbContext.Remove(carroAntigo);
            await _dbContext.SaveChangesAsync();
        }
        return carroAntigo;
    }

    private async Task PublishUpdateAsync(Carro carro){
        await this._daprClient.PublishEventAsync(_configuration["AppComponentService"], 
                                                _configuration["AppComponentTopicCarro"], 
                                                carro);
    }

}