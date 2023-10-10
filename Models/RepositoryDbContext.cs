using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace tapr_2023_equipe1_carro_dotnet.Models;

public class RepositoryDbContext : DbContext
{
    public DbSet<Carro> Carros {get;set;}

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
            .HasPartitionKey(o => o.placa);
        
    }
}
