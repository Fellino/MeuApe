using ApartmentPlanner.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentPlanner.Api.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<ApartmentMember> ApartmentMembers { get; set; }
    public DbSet<Contribution> Contributions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configurações adicionais, se necessário
        modelBuilder.Entity<Contribution>()
            .Property(c => c.Amount)
            .HasPrecision(18, 2);
    }
}