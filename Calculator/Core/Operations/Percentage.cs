using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

public class Percentage : IOperation
{
    public double Execute(double a, double b) => (a * b) / 100;
}