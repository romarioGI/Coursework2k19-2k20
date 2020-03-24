using System;
using System.Numerics;

namespace MathLib
{
    public class RationalNumber : AbstractNumber
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
            return first != null && first.Equals(second);
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

        public override Sign Sign { get; }

        protected override AbstractNumber AddNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return this + (RationalNumber)abstractNumber;
        }

        protected override AbstractNumber SubtractNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return this - (RationalNumber)abstractNumber;
        }

        protected override AbstractNumber GetOpposite()
        {
            return -1 * this;
        }

        protected override AbstractNumber MultiplyNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return this * (RationalNumber)abstractNumber;
        }

        protected override AbstractNumber DivideNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return this / (RationalNumber)abstractNumber;
        }

        protected override AbstractNumber GetRemainderNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return new RationalNumber(0, 1);
        }

        protected override bool EqualsNotZeroAndEqualType(AbstractNumber other)
        {
            return Equals((RationalNumber) other);
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
    }
}