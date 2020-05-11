using System;

namespace TarskiAlgorithmLib
{
    public class RationalAbstractNumber<T>:AbstractSigned, IEquatable<RationalAbstractNumber<T>> where T: AbstractNumber
    {
        private readonly T _numerator;
        private readonly T _denominator;

        public RationalAbstractNumber(T numerator, T denominator)
        {
            if(numerator is null || denominator is null)
                throw new ArgumentNullException();

            if (denominator.IsZero)
                throw new DivideByZeroException();

            _numerator = numerator;
            _denominator = denominator;

            if (numerator is AbstractEuclideanNumber numeratorE)
            {
                var denominatorE = denominator as AbstractEuclideanNumber;

                var gcd = AbstractEuclideanNumber.GreatestCommonDivisor(numeratorE, denominatorE) as T;
                _numerator = _numerator / gcd as T;
                _denominator = _denominator / gcd as T;
            }

            if (_numerator is null || _denominator is null)
                throw new ArgumentNullException();

            Sign = _numerator.Sign.Divide(_denominator.Sign);
        }

        public static RationalAbstractNumber<T> operator +(RationalAbstractNumber<T> first, RationalAbstractNumber<T> second)
        {
            var numerator = first._numerator * second._denominator + second._numerator * first._denominator;
            var denominator = first._denominator * second._denominator;

            return new RationalAbstractNumber<T>(numerator as T, denominator as T);
        }

        public static RationalAbstractNumber<T> operator -(RationalAbstractNumber<T> first, RationalAbstractNumber<T> second)
        {
            var numerator = first._numerator * second._denominator - second._numerator * first._denominator;
            var denominator = first._denominator * second._denominator;

            return new RationalAbstractNumber<T>(numerator as T, denominator as T);
        }

        public static RationalAbstractNumber<T> operator *(RationalAbstractNumber<T> first, RationalAbstractNumber<T> second)
        {
            var numerator = first._numerator * second._numerator;
            var denominator = first._denominator * second._denominator;

            return new RationalAbstractNumber<T>(numerator as T, denominator as T);
        }

        public RationalAbstractNumber<T> Inverse()
        {
            return new RationalAbstractNumber<T>(_denominator, _numerator);
        }

        public static RationalAbstractNumber<T> Inverse(RationalAbstractNumber<T> number)
        {
            return number.Inverse();
        }

        public static RationalAbstractNumber<T> operator /(RationalAbstractNumber<T> first, RationalAbstractNumber<T> second)
        {
            var numerator = first._numerator * second._denominator;
            var denominator = first._denominator * second._numerator;

            return new RationalAbstractNumber<T>(numerator as T, denominator as T);
        }

        public static bool operator ==(RationalAbstractNumber<T> first, RationalAbstractNumber<T> second)
        {
            return first != null && first.Equals(second);
        }

        public static bool operator !=(RationalAbstractNumber<T> first, RationalAbstractNumber<T> second)
        {
            return !(first == second);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_numerator, _denominator);
        }

        public bool Equals(RationalAbstractNumber<T> other)
        {
            return other != null && _numerator.Equals(other._numerator) && _denominator.Equals(other._denominator);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is RationalAbstractNumber<T> number) return Equals(number);
            return false;
        }

        public (T, T) GetNumeratorAndDenominator()
        {
            return (_numerator, _denominator);
        }
    }
}