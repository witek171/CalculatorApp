using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

public class Negation : IUnaryOperation
{
    public double Execute(double a) => -a;
}