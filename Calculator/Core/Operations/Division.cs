using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

public class Division : IOperation
{
    public double Execute(double a, double b) => a / b;
}