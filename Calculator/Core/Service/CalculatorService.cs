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
    // Flag indicating if the user is entering a new number after an operation or reset, if flag is false we can append a number to the result
    private bool _isNewEntry = true;
    private bool _isError;
    private const int MaxDisplayLength = 12;

    public string OperationExpression { get; private set; } = string.Empty;

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
        if (calculatorUtils.TryParseInput(input, out _previousValue))
        {
            _currentOperator = op;
            _isNewEntry = true;
            OperationExpression = $"{_previousValue} {_currentOperator}";
        }

        return input;
    }
    
    public (string result, string operationText) Calculate(string input)
    {
        if (!calculatorUtils.TryParseInput(input, out _currentValue))
            return (input, "");

        double result;
        string operationText;

        try
        {
            // calculate unary operation
            if (_unaryOperations.TryGetValue(_currentOperator, out var unaryOperation))
            {
                if (unaryOperation is Negation)
                    _isNewEntry = false;

                result = unaryOperation.Execute(_currentValue);
                operationText = $"{unaryOperation.Symbol}({_currentValue})";
            }
            // calculate binary operation
            else if (_operations.TryGetValue(_currentOperator, out var operation))
            {
                if (!_isNewEntry)
                {
                    result = operation.Execute(_previousValue, _currentValue);
                    operationText = $"{_previousValue} {_currentOperator} {_currentValue} =";
                    _previousValue = _currentValue;
                }
                else
                {
                    result = operation.Execute(_currentValue, _previousValue);
                    operationText = $"{_currentValue} {_currentOperator} {_previousValue} =";
                }

                _currentValue = result;
                _isNewEntry = true;
            }
            else return (input, "");
        }
        catch (Exception e)
        {
            _isError = true;
            return (e.Message, "");
        }

        return (calculatorUtils.FormatResult(result), operationText);
    }

    public string Clear(bool fullReset = true)
    {
        if (fullReset)
        {
            _previousValue = 0;
            _currentOperator = string.Empty;
        }

        _currentValue = 0;
        _isNewEntry = true;
        _isError = false;
        return "0";
    }

    public string ClearEnter() => Clear(fullReset: false);

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
        OperationExpression = string.Empty;
        return "0,";
    }

    public string Backspace(string input)
    {
        if (
            calculatorUtils.IsScientificNotation(input) ||
            (_isNewEntry && !_isError)
        )
            return input;

        if (
            input != "-0," &&
            !_isError &&
            input.Length != 1 &&
            !(input.Length == 2 && input.StartsWith('-'))
        )
            return input[..^1];

        _isError = false;
        return "0";
    }
}