using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Rate> Rates => Set<Rate>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}