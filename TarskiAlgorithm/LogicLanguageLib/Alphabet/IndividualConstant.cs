using System;

namespace LogicLanguageLib.Alphabet
{
    public class IndividualConstant<T> : NonLogicalSymbol, IEquatable<IndividualConstant<T>>
    {
        public readonly T Value;

        public IndividualConstant(T value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            Value = value;
        }

        public bool Equals(IndividualConstant<T> other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int Priority => -10;

        protected override bool EqualsSameType(Symbol other)
        {
            return Value.Equals(((IndividualConstant<T>)other).Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public static implicit operator T(IndividualConstant<T> constant)
        {
            return constant.Value;
        }

        public static implicit operator IndividualConstant<T>(T value)
        {
            return new IndividualConstant<T>(value);
        }
    }
}