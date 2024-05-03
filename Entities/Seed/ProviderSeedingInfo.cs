namespace Entities.Seed;

public class ProviderSeedingInfo(string id, int customerCount, string licensePlateFormat, string panFormat)
{
    /// <summary>
    /// Gets or sets the provider's id, consisting of a digit and country code
    /// </summary>
    public string Id { get; set; } = id;

    /// <summary>
    /// Gets or sets the number of customers (vehicles)
    /// </summary>
    public int CustomerCount { get; set; } = customerCount;

    /// <summary>
    /// Gets or sets licence plate format, including country code
    /// The format must at least generate <see cref="CustomerCount"/> entries.
    /// </summary>
    public string LicensePlateFormat { get; set; } = licensePlateFormat;

    /// <summary>
    /// Gets or sets the personal account number format.
    /// The format must at least generate <see cref="CustomerCount"/> entries.
    /// It must contain exactly 20 digits
    /// </summary>
    public string PanFormat { get; set; } = panFormat;
}