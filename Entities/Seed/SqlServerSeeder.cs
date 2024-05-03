using Microsoft.EntityFrameworkCore;

namespace Entities.Seed;

public class SqlServerSeeder : ISeeder
{
    public Task ClearAsync()
    {
        var ctx = new CustomerContext();
        ctx.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.Customers");
        return Task.CompletedTask;
    }

    public Task SeedAsync(ProviderSeedingInfo providerSeedingInfo)
    {
        var ctx = new CustomerContext();

        // Generate strings, representing the licence plate
        var licencePlateGenerator = new StringGenerator(new StringGeneratorInfo(providerSeedingInfo.LicensePlateFormat, providerSeedingInfo.CustomerCount));
        var licencePlates = licencePlateGenerator.Generate().ToList();

        var panGenerator = new StringGenerator(new StringGeneratorInfo(providerSeedingInfo.PanFormat, providerSeedingInfo.CustomerCount));
        var pans = panGenerator.Generate().ToList();

        var customers = Combine(providerSeedingInfo.Id, licencePlates, pans);

        ctx.BulkInsert<Customer>(customers);

        return Task.CompletedTask;
    }

    private IEnumerable<Customer> Combine(string providerId, List<string> licencePlates, List<string> pans)
    {
        for (var i = 0; i < licencePlates.Count; i++)
        {
            yield return new Customer(providerId, licencePlates[i], pans[i]);
        }
    }
}