using System;
using Calculator.Core.Interfaces;

namespace Calculator.Core.Operations;

public class Division : IOperation
{
    public string Symbol => "÷";

    public double Execute(double a, double b)
    {
        if (b == 0)
            throw new DivideByZeroException("Can not divide by 0");
        return a / b;
    }
}