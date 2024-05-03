using Microsoft.EntityFrameworkCore;

namespace Entities.Seed;

public class CustomerContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Database=dadummy;TrustServerCertificate=True;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasKey(customer => customer.LicensePlate);
        base.OnModelCreating(modelBuilder);
    }
}

public class Customer (string providerId, string licensePlate, string pan)
{
    /// <summary>
    /// Gets or sets the provider's id, consisting of a digit and country code
    /// </summary>
    public string ProviderId { get; set; } = providerId;

    /// <summary>
    /// Gets or sets licence plate, including country code
    /// </summary>
    public string LicensePlate { get; set; } = licensePlate;

    /// <summary>
    /// Gets or sets the personal account number.
    /// It must contain exactly 20 digits
    /// </summary>
    public string Pan { get; set; } = pan;
}