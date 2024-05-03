using Microsoft.EntityFrameworkCore;

namespace Entities.Seed;

public class CustomerContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("data source=.;initial catalog=dadummy;trusted_connection=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasKey(customer => customer.LicensePlate);
        base.OnModelCreating(modelBuilder);
    }
}

public class Customer
{
    /// <summary>
    /// Gets or sets the provider's id, consisting of a digit and country code
    /// </summary>
    public string ProviderId { get; set; }

    /// <summary>
    /// Gets or sets licence plate, including country code
    /// </summary>
    public string LicensePlate { get; set; }

    /// <summary>
    /// Gets or sets the personal account number.
    /// It must contain exactly 20 digits
    /// </summary>
    public string Pan { get; set; }
}