using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Calculator.Core.Providers;
using Calculator.Core.Service;
using Calculator.Core.Utilities;

namespace Calculator.Views;

public partial class MainWindow
{
    private readonly CalculatorService _calculator;

    public MainWindow()
    {
        InitializeComponent();
        var calculatorUtils = new CalculatorUtils();
        var operationProvider = new OperationProvider();
        _calculator = new CalculatorService(calculatorUtils, operationProvider);
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key >= Key.D0 && e.Key <= Key.D9)
        {
            var key = (e.Key - Key.D0).ToString();
            Display.Text = _calculator.ProcessNumber(Display.Text, key);
        }
    }

    private void Number_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var number = button.Content.ToString();
            Display.Text = _calculator.ProcessNumber(Display.Text, number!);
        }
    }

    private void Operator_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var operation = button.Content.ToString();
            Display.Text = _calculator.ProcessOperator(Display.Text, operation!);
            OperationDisplay.Text = _calculator.OperationExpression;
        }
    }

    private void Equals_Click(object sender, RoutedEventArgs e)
    {
        var (result, operationText) = _calculator.Calculate(Display.Text);
        Display.Text = result;
        OperationDisplay.Text = operationText;
    }

    private void UnaryOperator_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            var operation = button.Content.ToString();
            Display.Text = _calculator.ProcessOperator(Display.Text, operation!);
            OperationDisplay.Text = _calculator.OperationExpression;
        }

        var (result, operationText) = _calculator.Calculate(Display.Text);
        Display.Text = result;
        OperationDisplay.Text = operationText;
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        Display.Text = _calculator.Clear();
        OperationDisplay.Text = string.Empty;
    }
    
    private void ClearEnter_Click(object sender, RoutedEventArgs e)
    {
        Display.Text = _calculator.ClearEnter();
        OperationDisplay.Text = string.Empty;
    }

    private void Decimal_Click(object sender, RoutedEventArgs e)
    {
        Display.Text = _calculator.AddDecimal(Display.Text);
    }

    private void Backspace_Click(object sender, RoutedEventArgs e)
    {
        Display.Text = _calculator.Backspace(Display.Text);
    }

    private void Display_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            var length = textBox.Text.Length;

            textBox.FontSize = length switch
            {
                < 14 => 46,
                < 45 => 38,
                _ => 14.33
            };
        }
    }
}