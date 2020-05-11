using System;
using SimpleTarskiAlgorithmLib;

namespace TarskiAlgorithmLib
{
    public struct RationalMonomialsNumber
    {
        private readonly MonomialsNumber _numerator;
        private readonly MonomialsNumber _denominator;

        public readonly Sign Sign;

        public bool IsZero => Sign == Sign.Zero;

        public RationalMonomialsNumber(MonomialsNumber numerator, MonomialsNumber denominator)
        {
            if (denominator.Sign.HasFlag(Sign.Zero))
                throw new DivideByZeroException();

            _numerator = numerator;
            _denominator = denominator;

            Sign = _numerator.Sign.Divide(_denominator.Sign);
        }

        public static RationalMonomialsNumber operator +(RationalMonomialsNumber first, RationalMonomialsNumber second)
        {
            var numerator = first._numerator * second._denominator + second._numerator * first._denominator;
            var denominator = first._denominator * second._denominator;

            return new RationalMonomialsNumber(numerator, denominator);
        }

        public static RationalMonomialsNumber operator -(RationalMonomialsNumber first, RationalMonomialsNumber second)
        {
            var numerator = first._numerator * second._denominator - second._numerator * first._denominator;
            var denominator = first._denominator * second._denominator;

            return new RationalMonomialsNumber(numerator, denominator);
        }

        public static RationalMonomialsNumber operator *(RationalMonomialsNumber first, RationalMonomialsNumber second)
        {
            var numerator = first._numerator * second._numerator;
            var denominator = first._denominator * second._denominator;

            return new RationalMonomialsNumber(numerator, denominator);
        }

        public static RationalMonomialsNumber operator -(RationalMonomialsNumber first)
        {
            return new RationalMonomialsNumber(-first._numerator, first._denominator);
        }

        public RationalMonomialsNumber Inverse()
        {
            return new RationalMonomialsNumber(_denominator, _numerator);
        }

        public static RationalMonomialsNumber Inverse(RationalMonomialsNumber number)
        {
            return number.Inverse();
        }

        public static RationalMonomialsNumber operator /(RationalMonomialsNumber first, RationalMonomialsNumber second)
        {
            var numerator = first._numerator * second._denominator;
            var denominator = first._denominator * second._numerator;

            return new RationalMonomialsNumber(numerator, denominator);
        }

        public RationalMonomialsNumber Pow(int degree)
        {
            if (degree < 0)
                throw new ArgumentOutOfRangeException();

            RationalMonomialsNumber res = 1;
            var cur = this;

            while (degree != 0)
            {
                if (degree % 2 == 1)
                    res *= cur;

                degree /= 2;
                cur *= cur;
            }

            return res;
        }

        public static bool operator ==(RationalMonomialsNumber first, RationalMonomialsNumber second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(RationalMonomialsNumber first, RationalMonomialsNumber second)
        {
            return !(first == second);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_numerator, _denominator);
        }

        public bool Equals(RationalMonomialsNumber other)
        {
            return _numerator.Equals(other._numerator) && _denominator.Equals(other._denominator);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is RationalMonomialsNumber rationalMonomialsNumber && Equals(rationalMonomialsNumber);
        }

        public static implicit operator RationalMonomialsNumber(MonomialsNumber num)
        {
            return new RationalMonomialsNumber(num, 1);
        }

        public static implicit operator RationalMonomialsNumber(int num)
        {
            return new RationalMonomialsNumber(num, 1);
        }

        public override string ToString()
        {
            if (_denominator.Equals(1))
                return $"{_numerator}";
            return $"{_numerator}/{_denominator}";
        }
    }
}