using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTarskiAlgorithmLib;

namespace TarskiAlgorithmLib
{
    //TODO в словаре мб стоит избавиться от ключей с нулевым коеф, при этои всем добавлять пустой моном со значением 0
    public struct MonomialsNumber : IEquatable<MonomialsNumber>
    {
        private readonly Dictionary<Monomial, RationalNumber> _coefficients;

        public readonly Sign Sign;

        public static MonomialsNumber Zero => new MonomialsNumber(Monomial.EmptyMonomial, 0);

        public static MonomialsNumber One => new MonomialsNumber(Monomial.EmptyMonomial, 1);

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

        private MonomialsNumber(Dictionary<Monomial, RationalNumber> coefficients, Sign sign)
        {
            _coefficients = coefficients;
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
            var res = new Dictionary<Monomial, RationalNumber>();
            foreach (var p in first._coefficients)
                res.Add(p.Key, p.Value);

            foreach (var p in second._coefficients)
                if (res.ContainsKey(p.Key))
                    res[p.Key] += p.Value;
                else
                    res.Add(p.Key, p.Value);

            var sign = first.Sign.Add(second.Sign);

            return new MonomialsNumber(res, sign);
        }

        public static MonomialsNumber operator -(MonomialsNumber first, MonomialsNumber second)
        {
            var res = new Dictionary<Monomial, RationalNumber>();
            foreach (var p in first._coefficients)
                res.Add(p.Key, p.Value);

            foreach (var p in second._coefficients)
                if (res.ContainsKey(p.Key))
                    res[p.Key] -= p.Value;
                else
                    res.Add(p.Key, -p.Value);

            var sign = first.Sign.Subtract(second.Sign);

            return new MonomialsNumber(res, sign);
        }

        public static MonomialsNumber operator -(MonomialsNumber first)
        {
            var res = new Dictionary<Monomial, RationalNumber>();
            foreach (var p in first._coefficients)
                res.Add(p.Key, -p.Value);

            var sign = first.Sign.Invert();

            return new MonomialsNumber(res, sign);
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

            return new MonomialsNumber(res, sign);
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

        public MonomialsNumber SetSign(Sign sign)
        {
            if (!Sign.HasFlag(sign))
                throw new ArgumentException();

            return new MonomialsNumber(_coefficients, sign);
        }

        public override string ToString()
        {
            return string.Join(" + ",
                _coefficients.Select(p => $"{p.Value}*{p.Key}"));
        }
    }
}