using System;

namespace MathLib
{
    public class PolynomialObjectVariableException<T> : Exception where T : INumber
    {
        public PolynomialObjectVariableException(Polynomial<T> first, Polynomial<T> second) :
            base(GetMessage(first, second))
        {
        }

        private static string GetMessage(Polynomial<T> first, Polynomial<T> second)
        {
            return
                $"Polynomials have {first.ObjectVariable} and {second.ObjectVariable} ObjectVariables, but they must be equal";
        }
    }
}