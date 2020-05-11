using System;

namespace TarskiAlgorithmLib.Exceptions
{
    public class PolynomialMonomialVariableNameException : Exception
    {
        public PolynomialMonomialVariableNameException(PolynomialMonomial first, PolynomialMonomial second) :
            base(GetMessage(first, second))
        {
        }

        private static string GetMessage(PolynomialMonomial first, PolynomialMonomial second)
        {
            return
                $"Polynomials have {first.VariableDomain} and {second.VariableDomain} VariableDomain, but they must be equal";
        }
    }
}