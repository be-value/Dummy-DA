namespace Entities.Seed;

public interface ISeeder
{
    Task<Azure.Response> ClearAsync();
    Task SeedAsync();
}