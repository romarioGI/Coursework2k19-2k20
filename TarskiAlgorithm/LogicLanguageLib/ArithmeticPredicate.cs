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

        public override int GetHashCode() => base.GetHashCode();
    }
}