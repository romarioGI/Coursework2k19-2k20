using System;

namespace LogicLanguageLib
{
    public class Predicate : NonLogicalSymbol, IEquatable<Predicate>
    {
        public readonly int Arity;

        public Predicate(string name, int arity) : base(name)
        {
            Arity = arity;
        }

        public bool Equals(Predicate other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Arity == other.Arity && base.Equals(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Predicate) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Arity);
        }
    }
}