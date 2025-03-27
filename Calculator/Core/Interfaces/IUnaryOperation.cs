namespace Calculator.Core.Interfaces;

public interface IUnaryOperation 
{
    string Symbol { get; }
    double Execute(double a);
}