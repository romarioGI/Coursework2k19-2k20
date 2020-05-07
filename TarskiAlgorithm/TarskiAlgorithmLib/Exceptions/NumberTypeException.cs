using System;

namespace TarskiAlgorithmLib.Exceptions
{
    internal class NumberTypeException : Exception
    {
        public NumberTypeException(AbstractNumber first, AbstractNumber second) : base(GetMessage(first, second))
        {
        }

        private static string GetMessage(AbstractNumber first, AbstractNumber second)
        {
            return $"{first.GetType()} and {second.GetType()} are incompatible AbstractNumber types";
        }
    }
}