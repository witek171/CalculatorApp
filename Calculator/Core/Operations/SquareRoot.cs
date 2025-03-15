using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

public class SquareRoot : IUnaryOperation
{
    public string Symbol => "√";

    public double Execute(double a) => Math.Sqrt(a);
}