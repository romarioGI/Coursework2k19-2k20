using System;

namespace LogicLanguageLib
{
    public class IndividualConstant<T> : NonLogicalSymbol, IEquatable<IndividualConstant<T>>
    {
        public readonly T Value;

        public IndividualConstant(T value) : base(GetName(value))
        {
            Value = value;
        }

        private static string GetName(T value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            return value.ToString();
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