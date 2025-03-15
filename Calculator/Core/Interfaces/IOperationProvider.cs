namespace Calculator.Core.Interfaces;

public interface IOperationProvider
{
    IReadOnlyDictionary<string, IOperation> GetOperations();
    IReadOnlyDictionary<string, IUnaryOperation> GetUnaryOperations();
}