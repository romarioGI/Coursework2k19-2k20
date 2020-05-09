using System;
using System.Numerics;

namespace SimpleTarskiAlgorithmLib
{
    public struct RationalNumber : IEquatable<RationalNumber>
    {
        private readonly BigInteger _numerator;
        private readonly BigInteger _denominator;

        public readonly Sign Sign;

        public bool IsZero => Sign == Sign.Zero;

        public RationalNumber(BigInteger numerator, BigInteger denominator)
        {
            if (denominator == 0)
                throw new DivideByZeroException();

            var gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
            _numerator = numerator / gcd;
            _denominator = denominator / gcd;

            if (_denominator < 0)
            {
                _numerator *= -1;
                _denominator *= -1;
            }

            if (_numerator.IsZero)
                Sign = Sign.Zero;
            else if (_numerator > 0)
                Sign = Sign.MoreZero;
            else
                Sign = Sign.LessZero;
        }

        public static RationalNumber operator +(RationalNumber first, RationalNumber second)
        {
            var numerator = first._numerator * second._denominator + second._numerator * first._denominator;
            var denominator = first._denominator * second._denominator;

            return new RationalNumber(numerator, denominator);
        }

        public static RationalNumber operator -(RationalNumber first, RationalNumber second)
        {
            var numerator = first._numerator * second._denominator - second._numerator * first._denominator;
            var denominator = first._denominator * second._denominator;

            return new RationalNumber(numerator, denominator);
        }

        public static RationalNumber operator *(RationalNumber first, RationalNumber second)
        {
            var numerator = first._numerator * second._numerator;
            var denominator = first._denominator * second._denominator;

            return new RationalNumber(numerator, denominator);
        }

        public static RationalNumber operator -(RationalNumber first)
        {
            return new RationalNumber(-first._numerator, first._denominator);
        }

        public RationalNumber Inverse()
        {
            return new RationalNumber(_denominator, _numerator);
        }

        public static RationalNumber Inverse(RationalNumber number)
        {
            return number.Inverse();
        }

        public static RationalNumber operator /(RationalNumber first, RationalNumber second)
        {
            var numerator = first._numerator * second._denominator;
            var denominator = first._denominator * second._numerator;

            return new RationalNumber(numerator, denominator);
        }

        public static bool operator ==(RationalNumber first, RationalNumber second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(RationalNumber first, RationalNumber second)
        {
            return !(first == second);
        }

        public static bool operator <(RationalNumber first, RationalNumber second)
        {
            return (first - second).Sign == Sign.LessZero;
        }

        public static bool operator >(RationalNumber first, RationalNumber second)
        {
            return second < first;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_numerator, _denominator);
        }

        public bool Equals(RationalNumber other)
        {
            return _numerator.Equals(other._numerator) && _denominator.Equals(other._denominator);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is RationalNumber rationalNumber && Equals(rationalNumber);
        }

        public (BigInteger, BigInteger) GetNumeratorAndDenominator()
        {
            return (_numerator, _denominator);
        }

        public static implicit operator RationalNumber(BigInteger num)
        {
            return new RationalNumber(num, 1);
        }

        public static implicit operator RationalNumber(int num)
        {
            return new RationalNumber(num, 1);
        }

        public override string ToString()
        {
            return $"{_numerator}/{_denominator}";
        }
    }
}