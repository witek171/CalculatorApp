using Calculator.Core.Interfaces;

namespace Calculator.Core.Service;

public class CalculatorService
{
    private readonly ICalculatorUtils _calculatorUtils;
    private readonly IReadOnlyDictionary<string, IOperation> _operations;
    private readonly IReadOnlyDictionary<string, IUnaryOperation> _unaryOperations;
    
    private double _currentValue;
    private double _previousValue;
    private string _currentOperator = string.Empty;
    private bool _isNewEntry = true;
    private bool _isError;
    private const int MaxDisplayLength = 11;

    public CalculatorService(
        ICalculatorUtils calculatorUtils,
        IOperationProvider operationProvider
        )
    {
        _calculatorUtils = calculatorUtils;
        _operations = operationProvider.GetOperations();
        _unaryOperations = operationProvider.GetUnaryOperations();
    }

    public string ProcessNumber(string input, string number)
    {
        if (_isError)
        {
            _isError = false;
            return number;
        }

        if (_isNewEntry || input == "0")
        {
            input = number;
        }
        else if (input.Length < MaxDisplayLength)
        {
            input += number;
        }

        _isNewEntry = false;
        return input;
    }

    public string ProcessOperator(string input, string op)
    {
        if (!_calculatorUtils.TryParseInput(input, out _previousValue))
            return input;
        
        _currentOperator = op;
        _isNewEntry = true;
        return input;
    }

    public string Calculate(string input)
    {
        if (!_calculatorUtils.TryParseInput(input, out _currentValue) || string.IsNullOrEmpty(_currentOperator))
            return input;

        if (!_operations.TryGetValue(_currentOperator, out var operation))
            return input;

        double result;

        try
        {
            if (!_isNewEntry)
            {
                result = operation.Execute(_previousValue, _currentValue);
                _previousValue = _currentValue;
            }
            else
            {
                result = operation.Execute(_currentValue, _previousValue);
            }
        }
        catch (DivideByZeroException e)
        {
            _isError = true;
            return e.Message;
        }
        catch (Exception)
        {
            _isError = true;
            return "Error";
        }

        _currentValue = result;
        _isNewEntry = true;

        return _calculatorUtils.FormatResult(result);
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
            !_calculatorUtils.TryParseInput(input, out var value) ||
            System.Text.RegularExpressions.Regex.IsMatch(input, @"^0,?0*$")
        )
        {
            return input;
        }

        if (!_unaryOperations.TryGetValue("+/-", out var operation))
            return input;
        
        value = operation.Execute(value);
        return _calculatorUtils.FormatResult(value);
    }

    public string SquareRoot(string input)
    {
        if (!_calculatorUtils.TryParseInput(input, out var value))
        {
            _isError = true;
            return "Error";
        }

        if (!_unaryOperations.TryGetValue("√", out var operation)) 
            return input;

        try
        {
            value = operation.Execute(value);
        }
        catch (ArgumentException e)
        {
            _isError = true;
            return e.Message;
        }
        catch (Exception)
        {
            _isError = true;
            return "Error";
        }

        return _calculatorUtils.FormatResult(value);
    }

    public string AddDecimal(string input)
    {
        if (!_isNewEntry && !_isError)
            return input.Contains(',') || input.Length >= MaxDisplayLength ? input : input + ",";

        _isNewEntry = false;
        _isError = false;
        return "0,";
    }
}