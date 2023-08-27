using System.Reflection;

namespace HSE.Salaries;

/// <summary>
/// Справочники
/// </summary>
public static class Dictionaries
{
    private static IEnumerable<string> ReadResourceFile(string filename)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename);
        using var reader = new StreamReader(stream!);
        var lines = new List<string>();
        while (reader.ReadLine() is { } line)
            lines.Add(line);
        return lines;
    }


    /// <summary>
    /// List of ISO 3166 country codes
    /// Взят с https://raw.githubusercontent.com/lukes/ISO-3166-Countries-with-Regional-Codes/master/all/all.csv
    /// Разделители заменены на ; с помощью https://onlinecsvtools.com/change-csv-delimiter
    /// </summary>
    public static readonly Dictionary<string, string> Countries = ReadResourceFile("HSE.Salaries.CountryCodes.csv").Skip(1)
        .Select(x => x.Split(";")[..2])
        .Where(s => !string.IsNullOrEmpty(s[1]))
        .ToDictionary(strings => strings[1], strings => strings[0]);
        
    /// <summary>
    /// List of ISO 4217 currency code
    /// Взят с https://www.six-group.com/en/products-services/financial-information/data-standards.html
    /// Сохранен из XLS в CSV
    /// </summary>
    public static readonly HashSet<string> Currencies = ReadResourceFile("HSE.Salaries.CurrencyCodes.csv").Skip(1)
        .Select(x => x.Split(",")[2])
        .Where(s => !string.IsNullOrEmpty(s))
        .ToHashSet();
}