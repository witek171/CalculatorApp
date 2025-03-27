using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

public class Division : IOperation
{
    public double Execute(double a, double b)
    {
        if (b == 0)
            throw new DivideByZeroException("Cannot divide by 0");

        return a / b;
    }
}