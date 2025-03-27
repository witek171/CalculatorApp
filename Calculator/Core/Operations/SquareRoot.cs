using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

public class SquareRoot : IUnaryOperation
{
    public string Symbol => "√";

    public double Execute(double a)
    {
        if (a < 0)
            throw new ArgumentException("Cannot calculate square root of a negative number");

        return Math.Sqrt(a);
    }
}
