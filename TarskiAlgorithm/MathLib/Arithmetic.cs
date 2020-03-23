using System;

namespace MathLib
{
    public class Arithmetic<T> : IEquatable<Arithmetic<T>> where T : INumber
    {
        public static Arithmetic<T> Zero = new Arithmetic<T>(INumber.Zero);

        private readonly INumber _value;

        public T Value => (T) _value;

        public bool IsZero => _value.IsZero;

        public bool CanZero => _value.CanZero;

        public Arithmetic(T value)
        {
            if (value is null)
                throw new ArgumentNullException();
            _value = value;
        }

        private Arithmetic(INumber number)
        {
            _value = number;
        }

        public static Arithmetic<T> operator +(Arithmetic<T> first, Arithmetic<T> second)
        {
            return new Arithmetic<T>(first._value.Add(second._value));
        }

        public static Arithmetic<T> operator -(Arithmetic<T> first, Arithmetic<T> second)
        {
            return new Arithmetic<T>(first._value.Subtract(second._value));
        }

        public static Arithmetic<T> operator *(Arithmetic<T> first, Arithmetic<T> second)
        {
            return new Arithmetic<T>(first._value.Multiply(second._value));
        }

        public static Arithmetic<T> operator *(Arithmetic<T> first, int second)
        {
            return new Arithmetic<T>(first._value.Multiply(second));
        }

        public static Arithmetic<T> operator *(int first, Arithmetic<T> second)
        {
            return second * first;
        }

        public static Arithmetic<T> operator /(Arithmetic<T> first, Arithmetic<T> second)
        {
            return new Arithmetic<T>(first._value.Divide(second._value));
        }

        public static Arithmetic<T> operator %(Arithmetic<T> first, Arithmetic<T> second)
        {
            return new Arithmetic<T>(first._value.GetRemainder(second._value));
        }

        public static bool operator ==(Arithmetic<T> first, Arithmetic<T> second)
        {
            return first != null && first.Equals(second);
        }

        public static bool operator !=(Arithmetic<T> first, Arithmetic<T> second)
        {
            return !(first == second);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public bool Equals(Arithmetic<T> other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || _value.Equals(other._value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Arithmetic<T>) obj);
        }

        public static implicit operator Arithmetic<T>(T value)
        {
            if (value is null)
                throw new ArgumentNullException();
            return new Arithmetic<T>(value);
        }

        public static implicit operator T(Arithmetic<T> value)
        {
            if(value is null)
                throw new ArgumentNullException();
            return value.Value;
        }
    }
}