using System;

namespace MathLib.Exceptions
{
    public class PolynomialObjectVariableException<T> : Exception where T : AbstractNumber
    {
        public PolynomialObjectVariableException(Polynomial<T> first, Polynomial<T> second) :
            base(GetMessage(first, second))
        {
        }

        public PolynomialObjectVariableException(Polynomial<T> first) :
            base(GetMessage(first))
        {
        }

        private static string GetMessage(Polynomial<T> first, Polynomial<T> second)
        {
            return
                $"Polynomials have {first.VariableDomain} and {second.VariableDomain} ObjectVariables, but they must be equal";
        }

        private static string GetMessage(Polynomial<T> first)
        {
            return
                "Polynomial coefficients VariableDomain and Polynomial.VariableDomain.Children must be equal ";
        }
    }
}