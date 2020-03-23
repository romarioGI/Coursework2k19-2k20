using System;

namespace MathLib
{
    internal class NumberTypeException : Exception
    {
        public NumberTypeException(INumber first, INumber second) : base(GetMessage(first, second))
        {
        }

        private static string GetMessage(INumber first, INumber second)
        {
            return $"{first.GetType()} and {second.GetType()} are incompatible INumber types";
        }
    }
}