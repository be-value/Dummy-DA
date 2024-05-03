using Entities.Seed;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace Api;

public class DataReset(ISeeder seeder)
{
    [Function("DataReset")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "data")] HttpRequest req)
    {
        await seeder.ClearAsync();
        var providerSeedingInfo = new ProviderSeedingInfo("3-NL", 50000, "ll-ddd-l | NL", "1d2d3d4d5d");

        await seeder.SeedAsync(providerSeedingInfo);
        return new OkResult();
    }
}