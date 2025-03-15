using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

public class Negation : IUnaryOperation
{
    public string Symbol => "+/-";

    public double Execute(double a) => -a;
}