namespace Calculator.Core.Interfaces;

public interface ICalculatorUtils
{
    bool TryParseInput(string input, out double result);
    string FormatResult(double value);
    bool IsZeroInput(string input);
    bool IsScientificNotation(string input);
}