using System;

namespace LogicLanguageLib.Alphabet
{
    public class Predicate : NonLogicalSymbol, IEquatable<Predicate>
    {
        public readonly int Arity;

        private readonly string _name;

        public Predicate(string name, int arity)
        {
            Arity = arity;

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Predicate name must not be null or empty");
            _name = name;
        }

        public bool Equals(Predicate other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Arity == other.Arity && _name.Equals(other._name);
        }

        public override string ToString()
        {
            return _name;
        }

        public override int Priority => 70;

        protected override bool EqualsSameType(Symbol other)
        {
            var otherSameType = (Predicate) other;

            return Equals(otherSameType);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_name, Arity);
        }
    }
}