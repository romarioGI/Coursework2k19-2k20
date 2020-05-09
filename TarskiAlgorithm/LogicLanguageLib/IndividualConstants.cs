using System;

namespace LogicLanguageLib
{
    public class IndividualConstants<T> : NonLogicalSymbol, IEquatable<IndividualConstants<T>>
    {
        public readonly T Value;

        public IndividualConstants(T value) : base(value.ToString())
        {
            Value = value;
        }

        public bool Equals(IndividualConstants<T> other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((IndividualConstants<T>) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }
}