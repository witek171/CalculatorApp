using Calculator.Core.Interfaces;
using Calculator.Core.Operations;

namespace Calculator.Core.Service;

public class CalculatorService(
    ICalculatorUtils calculatorUtils,
    IOperationProvider operationProvider
)
{
    private readonly IReadOnlyDictionary<string, IOperation> _operations =
        operationProvider.GetOperations();

    private readonly IReadOnlyDictionary<string, IUnaryOperation> _unaryOperations =
        operationProvider.GetUnaryOperations();

    private double _currentValue;
    private double _previousValue;
    private string _currentOperator = string.Empty;
    private bool _isNewEntry = true;
    private bool _isError;
    private const int MaxDisplayLength = 12;

    public string ProcessNumber(string input, string number)
    {
        if (_isError)
        {
            _isError = false;
            return number;
        }

        if (
            _isNewEntry ||
            input == "0" ||
            calculatorUtils.IsScientificNotation(input)
        )
            input = number;

        else if (input.Length < MaxDisplayLength)
            input += number;

        _isNewEntry = false;
        return input;
    }

    public string ProcessOperator(string input, string op)
    {
        if (!calculatorUtils.TryParseInput(input, out _previousValue))
            return input;

        _currentOperator = op;
        _isNewEntry = true;
        return input;
    }

    public string Calculate(string input)
    {
        if (
            !calculatorUtils.TryParseInput(input, out _currentValue) ||
            string.IsNullOrEmpty(_currentOperator)
        )
            return input;

        if (!_operations.TryGetValue(_currentOperator, out var operation))
            return input;

        if (_currentValue == 0)
        {
            _isError = true;
            return "Can not divide by 0";
        }

        double result;

        if (!_isNewEntry)
        {
            result = operation.Execute(_previousValue, _currentValue);
            _previousValue = _currentValue;
        }
        else
            result = operation.Execute(_currentValue, _previousValue);

        _currentValue = result;
        _isNewEntry = true;
        return calculatorUtils.FormatResult(result);
    }

    public string Clear()
    {
        _currentValue = _previousValue = 0;
        _currentOperator = string.Empty;
        _isNewEntry = true;
        _isError = false;
        return "0";
    }

    public string Negate(string input)
    {
        if (
            !calculatorUtils.TryParseInput(input, out var value) ||
            calculatorUtils.IsZeroInput(input)
        )
            return input;

        var negationSymbol = _unaryOperations
            .FirstOrDefault(kvp => kvp.Value is Negation).Key;
        if (!_unaryOperations.TryGetValue(negationSymbol, out var operation))
            return input;

        value = operation.Execute(value);
        return calculatorUtils.FormatResult(value);
    }

    public string SquareRoot(string input)
    {
        if (!calculatorUtils.TryParseInput(input, out var value))
        {
            _isError = true;
            return "Error";
        }

        var squareRootSymbol = _unaryOperations
            .FirstOrDefault(kvp => kvp.Value is SquareRoot).Key;
        if (!_unaryOperations.TryGetValue(squareRootSymbol, out var operation))
            return input;

        if (value < 0)
        {
            _isError = true;
            return "Can not calculate square root of negative number";
        }

        value = operation.Execute(value);
        return calculatorUtils.FormatResult(value);
    }

    public string AddDecimal(string input)
    {
        if (
            !_isNewEntry &&
            !_isError &&
            !calculatorUtils.IsScientificNotation(input)
        )
            return input.Contains(',') || input.Length >= MaxDisplayLength ? input : input + ",";

        _isNewEntry = false;
        _isError = false;
        return "0,";
    }

    public string Backspace(string input)
    {
        if (calculatorUtils.IsScientificNotation(input))
            return input;

        if (
            input != "-0," &&
            !_isError &&
            input.Length != 1 &&
            !(input.Length == 2 && input.Contains('-'))
        )
            return input[..^1];

        _isError = false;
        return "0";
    }
}