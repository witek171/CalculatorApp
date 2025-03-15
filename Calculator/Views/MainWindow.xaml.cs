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
        var operationFactory = new OperationProvider();
        _calculator = new CalculatorService(calculatorUtils, operationFactory);
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
        }
    }

    private void Equals_Click(object sender, RoutedEventArgs e)
    {
        Display.Text = _calculator.Calculate(Display.Text);
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        Display.Text = _calculator.Clear();
    }

    private void Negate_Click(object sender, RoutedEventArgs e)
    {
        Display.Text = _calculator.Negate(Display.Text);
    }

    private void Sqrt_Click(object sender, RoutedEventArgs e)
    {
        Display.Text = _calculator.SquareRoot(Display.Text);
    }

    private void Decimal_Click(object sender, RoutedEventArgs e)
    {
        Display.Text = _calculator.AddDecimal(Display.Text);
    }

    private void Display_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            var length = textBox.Text.Length;

            textBox.FontSize = length switch
            {
                < 14 => 46,
                < 45 => 35,
                _ => 12.8
            };
        }
    }
}