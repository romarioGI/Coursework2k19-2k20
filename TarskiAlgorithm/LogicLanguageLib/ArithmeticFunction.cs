using System;

namespace LogicLanguageLib
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

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ArithmeticFunction) obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}