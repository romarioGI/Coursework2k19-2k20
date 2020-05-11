using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTarskiAlgorithmLib;

namespace TarskiAlgorithmLib
{
    public struct MonomialsNumber : IEquatable<MonomialsNumber>
    {
        private readonly Dictionary<Monomial, RationalNumber> _coefficients;

        public readonly Sign Sign;

        public MonomialsNumber(Monomial monomial, RationalNumber coefficient)
        {
            _coefficients = new Dictionary<Monomial, RationalNumber> {{monomial, coefficient}};

            if (monomial.Equals(Monomial.EmptyMonomial))
                Sign = coefficient.Sign;
            else
                Sign = Sign.Undefined;
        }

        public MonomialsNumber(Monomial monomial)
            : this(monomial, 1)
        {

        }

        private MonomialsNumber(IEnumerable<(Monomial, RationalNumber)> coefficients, Sign sign)
        {
            _coefficients = coefficients
                .Where(x => x.Item2 != 0)
                .ToDictionary(x => x.Item1, x => x.Item2);

            if (!_coefficients.ContainsKey(Monomial.EmptyMonomial))
                _coefficients.Add(Monomial.EmptyMonomial, 0);

            Sign = sign;
        }

        public static implicit operator MonomialsNumber(Monomial monomial)
        {
            return new MonomialsNumber(monomial);
        }

        public static implicit operator MonomialsNumber(RationalNumber rationalNumber)
        {
            return new MonomialsNumber(Monomial.EmptyMonomial, rationalNumber);
        }

        public static implicit operator MonomialsNumber(int number)
        {
            return (RationalNumber) number;
        }

        public static MonomialsNumber operator +(MonomialsNumber first, MonomialsNumber second)
        {
            var sign = first.Sign.Add(second.Sign);

            return new MonomialsNumber(GetSum(first, second), sign);
        }

        private static IEnumerable<(Monomial, RationalNumber)> GetSum(MonomialsNumber first, MonomialsNumber second)
        {
            foreach (var p in first._coefficients)
            {
                if (second._coefficients.ContainsKey(p.Key))
                    yield return (p.Key, p.Value + second._coefficients[p.Key]);
                else
                    yield return (p.Key, p.Value);
            }

            foreach (var p in second._coefficients)
            {
                if (!first._coefficients.ContainsKey(p.Key))
                    yield return (p.Key, p.Value);
            }
        }

        public static MonomialsNumber operator -(MonomialsNumber first, MonomialsNumber second)
        {
            var sign = first.Sign.Subtract(second.Sign);

            return new MonomialsNumber(GetDivide(first, second), sign);
        }

        private static IEnumerable<(Monomial, RationalNumber)> GetDivide(MonomialsNumber first, MonomialsNumber second)
        {
            foreach (var p in first._coefficients)
            {
                if (second._coefficients.ContainsKey(p.Key))
                    yield return (p.Key, p.Value - second._coefficients[p.Key]);
                else
                    yield return (p.Key, p.Value);
            }

            foreach (var p in second._coefficients)
            {
                if (!first._coefficients.ContainsKey(p.Key))
                    yield return (p.Key, -p.Value);
            }
        }

        public static MonomialsNumber operator -(MonomialsNumber first)
        {
            var sign = first.Sign.Invert();

            return new MonomialsNumber(GetInvert(first), sign);
        }

        private static IEnumerable<(Monomial, RationalNumber)> GetInvert(MonomialsNumber first)
        {
            return first._coefficients.Select(p => (p.Key, -p.Value));
        }

        public static MonomialsNumber operator *(MonomialsNumber first, MonomialsNumber second)
        {
            var res = new Dictionary<Monomial, RationalNumber>();
            foreach (var p1 in first._coefficients)
            foreach (var p2 in second._coefficients)
            {
                var monomial = p1.Key * p2.Key;
                var coefficient = p1.Value * p2.Value;

                if (res.ContainsKey(monomial))
                    res[monomial] += coefficient;
                else
                    res.Add(monomial, coefficient);
            }

            var sign = first.Sign.Multi(second.Sign);

            return new MonomialsNumber(res.Select(p => (p.Key, p.Value)), sign);
        }

        public bool Equals(MonomialsNumber other)
        {
            return _coefficients.SequenceEqual(other._coefficients);
        }

        public override bool Equals(object obj)
        {
            return obj is MonomialsNumber other && Equals(other);
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            foreach (var (key, value) in _coefficients)
                hashCode += HashCode.Combine(key, value);

            return hashCode;
        }

        public override string ToString()
        {
            return string.Join(" + ",
                _coefficients.Select(p => $"{p.Value}*{p.Key}"));
        }
    }
}