using System;
using System.Diagnostics.CodeAnalysis;

namespace Data;

public record Provider(ushort Number, string Country)
{
    public static Provider Rdw => new (6, "NL");

    public override string ToString()
    {
        return $"{Number}-{Country}";
    }

    /// <summary>
    /// Parse a key of format "00-XX" to a <see cref="Provider"/>
    /// </summary>
    /// <param name="key">A key of format "00-XX"</param>
    /// <returns>The parsed <see cref="Provider"/> instance</returns>
    /// <exception cref="ArgumentException"></exception>
    public static Provider Parse(string key)
    {
        if (!TryParse(key, out var provider))
        {
            throw new ArgumentException("Invalid value", nameof(key));
        }

        return provider;
    }

    /// <summary>
    /// Try to parse a key of format "00-XX" to a <see cref="Provider"/>
    /// </summary>
    /// <param name="key">A key of format "00-XX"</param>
    /// <param name="provider">The parsed <see cref="Provider"/> instance</param>
    /// <returns>True when key is valid; otherwise false</returns>
    public static bool TryParse(string? key, [NotNullWhen(true)] out Provider? provider)
    {
        provider = default;

        if (string.IsNullOrEmpty(key) || !key.Contains('-'))
        {
            return false;
        }

        var rowKey = key.Split('-');
        if (rowKey.Length != 2)
        {
            return false;
        }

        if (!ushort.TryParse(rowKey[0], out var value))
        {
            return false;
        }

        provider = new Provider(value, rowKey[1]);
        return true;
    }
}