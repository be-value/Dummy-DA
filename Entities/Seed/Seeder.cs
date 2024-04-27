using Azure.Data.Tables;
using Microsoft.Extensions.Logging;

namespace Entities.Seed;

internal class Seeder( ILogger<Seeder> logger) : ISeeder
{
    private const string ConnectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
    private const string LicencePlateTableName = "licenceplates";

    public Task<Azure.Response> ClearAsync()
    {
        TableClient licencePlates = new(ConnectionString, LicencePlateTableName);
        return licencePlates.DeleteAsync();
    }

    public async Task SeedAsync()
    {
        try
        {
            TableClient licencePlateTable = new(ConnectionString, LicencePlateTableName);
            await licencePlateTable.CreateIfNotExistsAsync();

            // Generate 800000 strings, representing the licence plate
            var stringGenerator = new StringGenerator(new StringGeneratorInfo("ll-ddd-l | NL", 50000));
            var licencePlates = stringGenerator.Generate().ToList();

            // Assign them all to the same provider
            var provider = "3-NL";

            var count = licencePlates.Count;
            var skip = 0;
            var batchSize = 100; // 100 = MAX. 101 produces error
            var batchCount = 0;

            do
            {
                var batch = licencePlates.Skip(skip).Take(batchSize);
                skip += batchSize;

                var transactionActions = new List<TableTransactionAction>();
                foreach (var licencePlate in batch)
                {
                    var licencePlateEntity = new LicencePlateEntity
                    {
                        PartitionKey = provider,
                        RowKey = licencePlate,
                        Pan = "some string"
                    };
                    transactionActions.Add(new TableTransactionAction(TableTransactionActionType.Add, licencePlateEntity));
                }

                await licencePlateTable.SubmitTransactionAsync(transactionActions);
                logger.LogInformation($"Inserted batch {++batchCount} int table.");

            } while (skip < count);

        }
        catch (Exception ex)
        {
            throw;
        }
    }
}