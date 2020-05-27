using System;

namespace LogicLanguageLib.Alphabet
{
    public class Function : NonLogicalSymbol, IEquatable<Function>
    {
        public readonly int Arity;

        private readonly string _name;

        public Function(string name, int arity)
        {
            Arity = arity;

            if(string.IsNullOrEmpty(name))
                throw new ArgumentException("Function name must not be null or empty");
            _name = name;
        }

        public bool Equals(Function other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Arity == other.Arity && _name.Equals(other._name);
        }

        public override string ToString()
        {
            return _name;
        }

        protected override bool EqualsSameType(Symbol other)
        {
            var otherSameType = (Function) other;
            return Arity == otherSameType.Arity && _name.Equals(otherSameType._name);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Arity, _name);
        }
    }
}