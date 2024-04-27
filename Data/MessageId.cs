using System;
using System.Diagnostics.CodeAnalysis;

namespace Data;

/// <summary>
/// Unique identification of a toll message between all Toll Collectors and Toll charger (Providers).
/// </summary>
/// <param name="Originator">The Provider where the message originated</param>
/// <param name="ApduId">The apduId as the unique identifier within the Provider system.</param>
public record MessageId(Provider Originator, ulong ApduId)
{
    public override string ToString()
    {
        return $"{Originator}:{ApduId}";
    }

    /// <summary>
    /// Parse a key of format "00-XX:yyyy" to a <see cref="MessageId"/>
    /// </summary>
    /// <param name="key">A key of format "00-XX:yyyy"</param>
    /// <returns>The parsed <see cref="MessageId"/> instance</returns>
    /// <exception cref="ArgumentException"></exception>
    public static MessageId Parse(string key)
    {
        if (!TryParse(key, out var messageId))
        {
            throw new ArgumentException("Invalid value", nameof(key));
        }

        return messageId;
    }

    /// <summary>
    /// Try to parse a key of format "00-XX:yyyy" to a <see cref="MessageId"/>
    /// </summary>
    /// <param name="key">A key of format "00-XX:yyyy"</param>
    /// <param name="messageId">The parsed <see cref="MessageId"/> instance</param>
    /// <returns>True when key is valid; otherwise false</returns>
    public static bool TryParse(string key, [NotNullWhen(true)] out MessageId? messageId)
    {
        messageId = default;

        if (!key.Contains(':'))
        {
            return false;
        }

        var parts = key.Split(':');
        if (parts.Length != 2)
        {
            return false;
        }

        if (!Provider.TryParse(parts[0], out var originator))
        {
            return false;
        }

        if (!ulong.TryParse(parts[1], out var apduId))
        {
            return false;
        }

        messageId = new MessageId(originator, apduId);
        return true;
    }
}