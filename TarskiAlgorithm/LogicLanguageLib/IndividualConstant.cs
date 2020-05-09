using System;

namespace LogicLanguageLib
{
    public class IndividualConstant<T> : NonLogicalSymbol, IEquatable<IndividualConstant<T>>
    {
        public readonly T Value;

        public IndividualConstant(T value) : base(value.ToString())
        {
            Value = value;
        }

        public bool Equals(IndividualConstant<T> other)
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
            return Equals((IndividualConstant<T>) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }
}