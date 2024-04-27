using Azure;
using Azure.Data.Tables;

namespace Entities;

public class LicencePlateEntity : ITableEntity
{
    public string PartitionKey { get; set; } = string.Empty;

    public string RowKey { get; set; } = string.Empty;

    public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.MinValue;

    public ETag ETag { get; set; } = ETag.All;

    public string Pan { get; set; } = string.Empty;
}