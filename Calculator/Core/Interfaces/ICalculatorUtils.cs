namespace Calculator.Core.Interfaces;

public interface ICalculatorUtils
{
    bool TryParseInput(string input, out double result);
    string FormatResult(double value);
}