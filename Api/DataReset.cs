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
        await seeder.SeedAsync();
        return new OkResult();
    }
}