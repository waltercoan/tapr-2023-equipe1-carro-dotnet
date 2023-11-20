using Microsoft.EntityFrameworkCore;
using tapr_2023_equipe1_carro_dotnet.Models;

namespace tapr_2023_equipe1_carro_dotnet.Services;

public class ClienteService : IClienteService
{

    private RepositoryDbContext _dbContext;
    public ClienteService(RepositoryDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<List<Cliente>> GetAllAsync()
    {
        var listaClientes = await _dbContext.Clientes.ToListAsync();
        return listaClientes;
    }

    public async Task<Cliente> GetByIdAsync(string id)
    {
        var cliente = await _dbContext.Clientes.Where(c => c.id.Equals(new Guid(id))).FirstOrDefaultAsync();
        return cliente;
    }

    public async Task<Cliente> saveNewAsync(Cliente cliente)
    {
        cliente.id = Guid.Empty;
        await _dbContext.Clientes.AddAsync(cliente);
        await _dbContext.SaveChangesAsync();

        return cliente;
    }

    public async Task<Cliente> updateAsync(string id, Cliente cliente)
    {
        var clienteAntigo = await _dbContext.Clientes.Where(c => c.id.Equals(new Guid(id))).FirstOrDefaultAsync();
        if (clienteAntigo != null){
            //Atualizar cada atributo do objeto antigo 
            clienteAntigo.Nome = cliente.Nome;
            clienteAntigo.Endereco = cliente.Endereco;
            await _dbContext.SaveChangesAsync();
        }
        return clienteAntigo;
    }
    public async Task<Cliente> DeleteAsync(string id)
    {
        var clienteAntigo = await _dbContext.Clientes.Where(c => c.id.Equals(new Guid(id))).FirstOrDefaultAsync();
        if (clienteAntigo != null){
             _dbContext.Remove(clienteAntigo);
            await _dbContext.SaveChangesAsync();
        }
        return clienteAntigo;
    }

    public async Task<Cliente> updateEventAsync(Cliente cliente)
    {
        var clienteAntigo = await _dbContext.Clientes.Where(c => c.id.Equals(cliente.id)).FirstOrDefaultAsync();
        if (clienteAntigo == null){
            await _dbContext.Clientes.AddAsync(cliente);
            await _dbContext.SaveChangesAsync();
        }else{
            await updateAsync(cliente.id.ToString(),cliente);
        }
        return cliente;
    }
}