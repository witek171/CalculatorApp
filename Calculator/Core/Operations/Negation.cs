using Calculator.Core.Interfaces;
using Calculator.Core.Providers;

namespace Calculator.Core.Operations;

public class Negation : IUnaryOperation
{
    public string Symbol => "negate";

    public double Execute(double a)
    {
        if (a == 0) 
            return 0; 
        
        return -a;
    }
}