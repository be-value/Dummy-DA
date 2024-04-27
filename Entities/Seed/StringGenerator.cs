namespace Entities.Seed;

/// <summary>
/// Use this class to generate a large set of strings for testing purposes
/// <see cref="StringGeneratorInfo"/> for specifics
/// </summary>
public class StringGenerator(StringGeneratorInfo generatorInfo)
{
    public IEnumerable<string> Generate()
    {
        var format = generatorInfo.Format;
        return GenerateRecursive(format).Take(generatorInfo.Count);
    }

    private IEnumerable<string> GenerateRecursive(string format)
    {
        if (format.Length == 1)
        {
            return generatorInfo.GetPermutations(format);
        }

        var parentVariations = GenerateRecursive(format.Substring(0, 1));
        var childVariations = GenerateRecursive(format.Substring(1)).ToList();

        return (from parentVariation in parentVariations from childVariation in childVariations 
            select string.Concat(parentVariation, childVariation));
    }
}