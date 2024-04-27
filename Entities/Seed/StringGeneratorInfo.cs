namespace Entities.Seed;

/// <summary>
/// Specifies how to generate strings using the <see cref="StringGenerator"/>
/// The format parameter must be a string containing only the following characters:
/// * 'b' : depicts a position for binary values (0 or 1)
/// * 'd' : depicts a position for numerical values (digits)
/// * 'l' : depicts a position for alphanumerical values (letters)
/// * 'h' : depicts a position for hexadecimal values (hex)
///
/// NOTE: use with care, this generator can generate huge amounts of strings.
/// For example: the generatorInfo string "ll ddd l" generates 26*26*10*10*10*26 = 17.576.000 entries.
/// Specifying characters other than noted above will yield always that character
/// on the specified position.
///
/// The entries are ordered. i.e. the generatorInfo "x ll y ddd z j" produces the following ordered set:
/// 
/// - x 00 y AAA z 0
/// - x 00 y AAA z 1
/// - ....
/// - x 00 y AAB z 0
/// - x 00 y AAB z 1
/// - ...
/// - x 99 y ZZZ z 8
/// - x 99 y ZZZ z 9
/// 
/// </summary>
/// <returns>The list of generated strings</returns>
public record StringGeneratorInfo
{
    public StringGeneratorInfo(string format, int count, int maximum = 20000000)
    {
        Format = format;
        Count = count;
        Maximum = maximum;

        GuardRange();
    }

    public string Format { get; set; }

    public int Count { get; set; }

    public int Maximum { get; set; }

    public List<string> GetPermutations(string format)
    {
        switch (format[0])
        {
            case 'b':
                return [..AllBits];
            case 'd':
                return [..AllDigits];
            case 'l':
                return [..AllLetters];
            case 'h':
                return [..AllHex];
            default:
                return [format];
        }
    }

    private static IEnumerable<string> AllBits => new List<string> { "0", "1" };

    private static IEnumerable<string> AllDigits => new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

    private static IEnumerable<string> AllLetters => new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

    private static IEnumerable<string> AllHex => new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };

    private void GuardRange()
    {
        var factors = new List<int>();
        foreach (var c in Format)
        {
            switch (c)
            {
                case 'b':
                    factors.Add(AllBits.Count());
                    break;
                case 'd':
                    factors.Add(AllDigits.Count());
                    break;
                case 'l':
                    factors.Add(AllLetters.Count());
                    break;
                case 'h':
                    factors.Add(AllHex.Count());
                    break;
            }
        }

        var generated = 1;
        foreach (var factor in factors)
        {
            if (Maximum / factor < generated)
            {
                throw new ArgumentOutOfRangeException($"Number of results to generate exceeds maximum of {Maximum} entries");
            }

            generated *= factor;
        }

        if (generated < this.Count)
        {
            throw new ArgumentOutOfRangeException($"Number of possible results is less than number of requested ({this.Count}) entries");
        }
    }
}