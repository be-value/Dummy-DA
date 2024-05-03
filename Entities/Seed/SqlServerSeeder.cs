using Azure;

namespace Entities.Seed;

public class SqlServerSeeder : ISeeder
{
    public Task<Response> ClearAsync()
    {
        throw new NotImplementedException();
    }

    public Task SeedAsync(ProviderSeedingInfo providerSeedingInfo)
    {
        throw new NotImplementedException();
    }
}