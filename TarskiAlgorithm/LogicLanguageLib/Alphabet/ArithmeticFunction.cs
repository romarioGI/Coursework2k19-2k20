using System;

namespace LogicLanguageLib.Alphabet
{
    public class ArithmeticFunction : Function, IEquatable<ArithmeticFunction>
    {
        public ArithmeticFunction(string name) : base(name, 2)
        {
        }

        public bool Equals(ArithmeticFunction other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}