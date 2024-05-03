using Azure.Data.Tables;
using Microsoft.Extensions.Logging;

namespace Entities.Seed;

internal class TableStorageSeeder( ILogger<TableStorageSeeder> logger) : ISeeder
{
    private const string ConnectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
    private const string LicencePlateTableName = "licenceplates";

    public Task<Azure.Response> ClearAsync()
    {
        TableClient licencePlates = new(ConnectionString, LicencePlateTableName);
        return licencePlates.DeleteAsync();
    }

    public async Task SeedAsync(ProviderSeedingInfo providerSeedingInfo)
    {
        try
        {
            TableClient licencePlateTable = new(ConnectionString, LicencePlateTableName);
            await licencePlateTable.CreateIfNotExistsAsync();

            // Generate strings, representing the licence plate
            var licencePlateGenerator = new StringGenerator(new StringGeneratorInfo(providerSeedingInfo.LicensePlateFormat, providerSeedingInfo.CustomerCount));
            var licencePlates = licencePlateGenerator.Generate().ToList();

            var panGenerator = new StringGenerator(new StringGeneratorInfo(providerSeedingInfo.PanFormat, providerSeedingInfo.CustomerCount));
            var pans = panGenerator.Generate().ToList();

            var customerInfos = Combine(licencePlates, pans);

            // Assign them all to the same provider
            var provider = providerSeedingInfo.Id;

            var count = providerSeedingInfo.CustomerCount;
            var skip = 0;
            var batchSize = 100; // 100 = MAX. 101 produces error
            var batchCount = 0;

            do
            {
                var batch = customerInfos.Skip(skip).Take(batchSize);
                skip += batchSize;

                var transactionActions = new List<TableTransactionAction>();
                foreach (var customerInfo in batch)
                {
                    var licencePlateEntity = new LicencePlateEntity
                    {
                        PartitionKey = provider,
                        RowKey = customerInfo.LicencePlate,
                        Pan = customerInfo.Pan
                    };
                    transactionActions.Add(new TableTransactionAction(TableTransactionActionType.Add, licencePlateEntity));
                }

                await licencePlateTable.SubmitTransactionAsync(transactionActions);
                logger.LogInformation($"Inserted batch {++batchCount} in table.");

            } while (skip < count);

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private IEnumerable<(string LicencePlate, string Pan)> Combine(List<string> licencePlates, List<string> pans)
    {
        for (var i = 0; i < licencePlates.Count; i++)
        {
            yield return new(licencePlates[i], pans[i]);
        }
    }
}