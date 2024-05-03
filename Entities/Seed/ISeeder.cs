namespace Entities.Seed;

public interface ISeeder
{
    Task ClearAsync();
    Task SeedAsync(ProviderSeedingInfo providerSeedingInfo);
}