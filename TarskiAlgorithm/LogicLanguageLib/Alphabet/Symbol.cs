using System;

namespace LogicLanguageLib.Alphabet
{
    public abstract class Symbol : IEquatable<Symbol>
    {
        public abstract override string ToString();

        public bool Equals(Symbol other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            return EqualsSameType(other);
        }

        protected abstract bool EqualsSameType(Symbol other);

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Symbol) obj);
        }

        public abstract override int GetHashCode();
    }
}