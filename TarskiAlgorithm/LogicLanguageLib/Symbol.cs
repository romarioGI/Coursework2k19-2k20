using System;

namespace LogicLanguageLib
{
    public abstract class Symbol : IEquatable<Symbol>
    {
        public readonly string Name;

        protected Symbol(string name)
        {
            Name = name ?? throw new ArgumentNullException();
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Symbol other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Symbol) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}