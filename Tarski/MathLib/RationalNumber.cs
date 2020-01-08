using System;
using System.Numerics;

namespace MathLib
{
    public struct RationalNumber
    {
        private readonly BigInteger _numerator;
        private readonly BigInteger _denominator;

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

        public bool IsZero()
        {
            return _numerator.IsZero;
        }

        public static bool operator ==(RationalNumber first, RationalNumber second)
        {
            return first._numerator == second._numerator && first._denominator == second._denominator;
        }

        public static bool operator !=(RationalNumber first, RationalNumber second)
        {
            return !(first == second);
        }

        public bool LessZero()
        {
            return _numerator < 0;
        }

        public static bool operator <(RationalNumber first, RationalNumber second)
        {
            return (first - second).LessZero();
        }

        public static bool operator >(RationalNumber first, RationalNumber second)
        {
            return second < first;
        }

        public override int GetHashCode()
        {
            return (_numerator.GetHashCode() * 397) ^ _denominator.GetHashCode();
        }

        public bool Equals(RationalNumber other)
        {
            return _numerator.Equals(other._numerator) && _denominator.Equals(other._denominator);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is RationalNumber other && Equals(other);
        }

        public (BigInteger, BigInteger) GetNumeratorAndDenominator()
        {
            return (_numerator, _denominator);
        }
    }
}