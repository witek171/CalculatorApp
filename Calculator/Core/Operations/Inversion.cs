using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

public class Inversion : IUnaryOperation
{
    public string Symbol => "1/";

    public double Execute(double a)
    {
        if (a == 0)
            throw new ArgumentException("Cannot invert 0");

        return 1 / a;
    }
}