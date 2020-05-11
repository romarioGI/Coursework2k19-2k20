using System;

namespace LogicLanguageLib
{
    public class ArithmeticPredicate : Predicate, IEquatable<ArithmeticPredicate>
    {
        public ArithmeticPredicate(string name) : base(name, 2)
        {
        }

        public bool Equals(ArithmeticPredicate other)
        {
            return base.Equals(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ArithmeticPredicate) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode());
        }
    }
}