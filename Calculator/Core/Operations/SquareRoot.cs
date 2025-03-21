using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

public class SquareRoot : IUnaryOperation
{
    public double Execute(double a) => Math.Sqrt(a);
}