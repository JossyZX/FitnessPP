using FitnessPP.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FitnessPP;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Locker> Lockers { get; set; }
    public DbSet<Service> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>()
            .HasOne<Trainer>()
            .WithMany()
            .HasForeignKey(c => c.TrainerId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Client>()
            .HasOne(c => c.Locker)
            .WithOne(l => l.Client)
            .HasForeignKey<Client>(c => c.LockerId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Locker>()
            .HasOne(l => l.Client)
            .WithOne(c => c.Locker)
            .HasForeignKey<Locker>(l => l.ClientId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Locker>()
            .HasIndex(l => l.Number)
            .IsUnique();

        modelBuilder.Entity<Client>()
            .HasMany(c => c.Services)
            .WithMany(s => s.Clients)
            .UsingEntity<Dictionary<string, object>>(
                "Client_Service", // Название таблицы по ТЗ 4.3
                j => j.HasOne<Service>().WithMany().HasForeignKey("service_id"),
                j => j.HasOne<Client>().WithMany().HasForeignKey("client_id")
            );

        // SEED-ДАННЫЕ (Автоматическое наполнение базы по ТЗ 2.4 и 4.4)

        var defaultServices = new List<Service>
        {
            new Service { Id = "SOLARIUM", Name = "Солярий", Price = 400 },
            new Service { Id = "POOL", Name = "Бассейн", Price = 200 },
            new Service { Id = "SAUNA", Name = "Сауна", Price = 0 },
            new Service { Id = "CRYOSAUNA", Name = "Криосауна", Price = 1000 },
            new Service { Id = "CROSSFIT", Name = "Кроссфит", Price = 500 }
        };
        modelBuilder.Entity<Service>().HasData(defaultServices);

        var defaultLockers = new List<Locker>();
        for (int i = 1; i <= 20; i++)
        {
            defaultLockers.Add(new Locker
            {
                Id = Guid.NewGuid(),
                Number = i,
                ClientId = null
            });
        }
        modelBuilder.Entity<Locker>().HasData(defaultLockers);
    }
    public class AppDbContextFactory : Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite("Data Source=fitness.db");

            return new AppDbContext(optionsBuilder.Options);
        }
    }

}
