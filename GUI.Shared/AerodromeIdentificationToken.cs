using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRumsey.OzStripsPlugin.GUI.Shared;

/// <summary>
/// Represents an aerodrome identification token (AIT) consisting of an ICAO code and an optional suffix.
/// </summary>
public class AerodromeIdentificationToken
{
    private string _icaoCode = string.Empty;

    private string _suffix = string.Empty;

    /// <summary>
    /// Gets or sets the ICAO code of the aerodrome. The ICAO code must be exactly 4 characters long and consist of letters only.
    /// </summary>
    public string ICAOCode
    {
        get => _icaoCode;
        set
        {
            if (value.Length != 4 || value.Any(x => !char.IsLetter(x)))
            {
                throw new ArgumentException("ICAO code must be 4 characters long.");
            }

            _icaoCode = value.ToUpperInvariant();
        }
    }

    /// <summary>
    /// Gets or sets the suffix of the aerodrome identification token.
    /// </summary>
    public string Suffix
    {
        get => _suffix;
        set
        {
            if (value.Any(x => !char.IsLetterOrDigit(x)))
            {
                throw new ArgumentException("Suffix must be alphanumeric characters");
            }

            _suffix = value.ToLowerInvariant();
        }
    }

    /// <summary>
    /// Gets a value indicating whether this AIT represents a sweatbox connection.
    /// </summary>
    public bool IsSweatbox => !string.IsNullOrEmpty(Suffix);

    /// <summary>
    /// Gets or sets the old server number associated with this AIT. The server number must be between 1 and 3.
    /// </summary>
    public int OldServerNumber
    {
        get
        {
            if (int.TryParse(Suffix, out var serverNumber) && serverNumber is >= 1 and <= 3)
            {
                return serverNumber;
            }
            else
            {
                return 0;
            }
        }

        set
        {
            if (value is < 1 or > 3)
            {
                return;
            }

            Suffix = value.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Gets a string representation of the aerodrome identification token, combining the ICAO code and suffix.
    /// </summary>
    public string StringToken => ICAOCode + Suffix;

    /// <summary>
    /// Determines whether the specified AIT matches this AIT based on ICAO code and suffix.
    /// </summary>
    /// <param name="other">To compare.</param>
    /// <returns>Is a match.</returns>
    public bool IsMatch(AerodromeIdentificationToken other)
    {
        if (other is null)
        {
            return false;
        }

        return ICAOCode == other.ICAOCode && Suffix == other.Suffix;
    }
}
