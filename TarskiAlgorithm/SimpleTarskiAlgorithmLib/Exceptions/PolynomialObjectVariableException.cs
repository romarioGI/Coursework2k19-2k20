using System;

namespace SimpleTarskiAlgorithmLib.Exceptions
{
    public class PolynomialObjectVariableException: Exception
    {
        public PolynomialObjectVariableException(Polynomial first, Polynomial second) :
            base(GetMessage(first, second))
        {
        }

        private static string GetMessage(Polynomial first, Polynomial second)
        {
            return
                $"Polynomials have {first.VariableName} and {second.VariableName} VariableNames, but they must be equal";
        }
    }
}