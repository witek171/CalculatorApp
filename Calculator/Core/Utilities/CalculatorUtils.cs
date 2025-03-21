using System.Globalization;
using Calculator.Core.Interfaces;
using Calculator.Core.Providers;

namespace Calculator.Core.Utilities;

public class CalculatorUtils : ICalculatorUtils
{
    public bool TryParseInput(string input, out double result)
    {
        return double.TryParse(
            input.Replace(",", "."),
            NumberStyles.Any, 
            CultureInfo.InvariantCulture,
            out result
            );
    }

    public string FormatResult(double value)
    {
        if (Math.Abs(value) >= 1e12 || (Math.Abs(value) < 1e-3 && value != 0))
        {
            return value.ToString("0.#####e+0", CultureInfo.InvariantCulture)
                .Replace(".", ",");
        }

        var formatted = value.ToString("G12", CultureInfo.InvariantCulture)
            .Replace(".", ",");
        var maxLength = value < 0 ? 13 : 12;

        return formatted.Length > maxLength ? formatted[..maxLength] : formatted;
    }
    
    public bool IsZeroInput(string input)
    {
        return RegexProvider.ZeroRegex().IsMatch(input);
    }

    public bool IsScientificNotation(string input)
    {
        return RegexProvider.ScientificNotationRegex().IsMatch(input);
    }
}