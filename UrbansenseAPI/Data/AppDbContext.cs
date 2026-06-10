using Microsoft.EntityFrameworkCore;
using UrbansenseAPI.Domain.Models;
using UrbansenseAPI.Infrastructure.Persistence.Mapping;

namespace UrbansenseAPI.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser>     Users          => Set<AppUser>();
    public DbSet<Alert>       Alerts         => Set<Alert>();
    public DbSet<Weather>     WeatherRecords => Set<Weather>();
    public DbSet<TransitLine> TransitLines   => Set<TransitLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMapping());
        modelBuilder.ApplyConfiguration(new AlertMapping());
        modelBuilder.ApplyConfiguration(new WeatherMapping());
        modelBuilder.ApplyConfiguration(new TransitLineMapping());

        base.OnModelCreating(modelBuilder);
    }
}
