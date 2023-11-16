using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace tapr_2023_equipe1_carro_dotnet.Models;

public class RepositoryDbContext : DbContext
{
    public DbSet<Carro> Carros {get;set;}
    public DbSet<Cliente> Clientes {get;set;}
    private IConfiguration _configuration;
    public RepositoryDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseCosmos(
            accountEndpoint: _configuration["CosmosDBURL"],
            tokenCredential: new DefaultAzureCredential(),
            databaseName: _configuration["CosmosDBDBName"],
            options => { options.ConnectionMode(ConnectionMode.Gateway); });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carro>()
            .HasNoDiscriminator();
        modelBuilder.Entity<Carro>()
            .ToContainer("carro");
        modelBuilder.Entity<Carro>()
            .Property(p => p.id)
            .HasValueGenerator<GuidValueGenerator>();
        modelBuilder.Entity<Carro>()
            .HasPartitionKey(o => o.id);

        modelBuilder.Entity<Cliente>()
            .HasNoDiscriminator();
        modelBuilder.Entity<Cliente>()
            .ToContainer("cliente");
        modelBuilder.Entity<Cliente>()
            .Property(p => p.id)
            .HasValueGenerator<GuidValueGenerator>();
        modelBuilder.Entity<Cliente>()
            .HasPartitionKey(o => o.id);
        
    }
}
