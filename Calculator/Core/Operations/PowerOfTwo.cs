using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

public class PowerOfTwo : IUnaryOperation
{
    public string Symbol => "sqr";

    public double Execute(double a) => Math.Pow(a, 2);
}