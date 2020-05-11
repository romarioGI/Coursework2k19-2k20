using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TarskiAlgorithmLib.Exceptions;

namespace TarskiAlgorithmLib
{
    public class PolynomialMonomial : IEquatable<PolynomialMonomial>
    {
        private readonly RationalMonomialsNumber[] _coefficients;
        private readonly int _hashCode;

        public readonly VariableName VariableDomain;

        public bool IsZero => Degree == -1;

        public PolynomialMonomial(IEnumerable<RationalMonomialsNumber> coefficients, VariableName variableDomain) :
            this(coefficients?.ToArray(), variableDomain)
        {
        }

        private PolynomialMonomial(RationalMonomialsNumber[] coefficients, VariableName variableDomain)
        {
            VariableDomain = variableDomain;

            if (coefficients is null)
                throw new ArgumentNullException(nameof(coefficients));

            var degree = coefficients.Length - 1;
            while (degree >= 0 && coefficients[degree].IsZero)
                --degree;

            _coefficients = new RationalMonomialsNumber[degree + 1];
            for (var i = 0; i <= degree; ++i)
                _coefficients[i] = coefficients[i];

            Degree = degree;
            _hashCode = CalcHashCode();
        }

        public readonly int Degree;

        private RationalMonomialsNumber this[int degree]
        {
            get
            {
                if (degree < 0 || degree > Degree)
                    throw new ArgumentOutOfRangeException();

                return _coefficients[degree];
            }
        }

        public RationalMonomialsNumber Leading => _coefficients[Degree];

        public IEnumerable<RationalMonomialsNumber> Coefficients => _coefficients.Select(c => c);

        public static PolynomialMonomial operator +(PolynomialMonomial f, PolynomialMonomial g)
        {
            if (f is null || g is null)
                throw new ArgumentNullException();

            if (!f.VariableDomain.Equals(g.VariableDomain))
                throw new PolynomialMonomialVariableNameException(f, g);

            var result = new RationalMonomialsNumber[Math.Max(f.Degree, g.Degree) + 1];
            var minDegree = Math.Min(f.Degree, g.Degree);
            for (var d = 0; d <= minDegree; ++d)
                result[d] = f[d] + g[d];

            for (var d = minDegree + 1; d <= f.Degree; ++d)
                result[d] = f[d];
            for (var d = minDegree + 1; d <= g.Degree; ++d)
                result[d] = g[d];

            return new PolynomialMonomial(result, f.VariableDomain);
        }

        public static PolynomialMonomial operator -(PolynomialMonomial f, PolynomialMonomial g)
        {
            if (f is null || g is null)
                throw new ArgumentNullException();

            if (!f.VariableDomain.Equals(g.VariableDomain))
                throw new PolynomialMonomialVariableNameException(f, g);

            var result = new RationalMonomialsNumber[Math.Max(f.Degree, g.Degree) + 1];
            var minDegree = Math.Min(f.Degree, g.Degree);
            for (var d = 0; d <= minDegree; ++d)
                result[d] = f[d] - g[d];

            for (var d = minDegree + 1; d <= f.Degree; ++d)
                result[d] = f[d];
            for (var d = minDegree + 1; d <= g.Degree; ++d)
                result[d] = -g[d];

            return new PolynomialMonomial(result, f.VariableDomain);
        }

        public static PolynomialMonomial operator -(PolynomialMonomial f)
        {
            if (f is null)
                throw new ArgumentNullException();

            var result = f._coefficients.Select(c => -c);

            return new PolynomialMonomial(result, f.VariableDomain);
        }

        public static PolynomialMonomial operator *(PolynomialMonomial f, PolynomialMonomial g)
        {
            if (f is null || g is null)
                throw new ArgumentNullException();

            if (!f.VariableDomain.Equals(g.VariableDomain))
                throw new PolynomialMonomialVariableNameException(f, g);

            if (f.IsZero)
                return f;
            if (g.IsZero)
                return g;

            var result = new RationalMonomialsNumber[f.Degree + g.Degree + 1];
            for (var i = 0; i < result.Length; ++i)
                result[i] = new RationalMonomialsNumber(0, 1);

            for (var d1 = 0; d1 <= f.Degree; ++d1)
            for (var d2 = 0; d2 <= g.Degree; ++d2)
                result[d1 + d2] += f[d1] * g[d2];

            return new PolynomialMonomial(result, f.VariableDomain);
        }

        public static PolynomialMonomial operator *(PolynomialMonomial f, int a)
        {
            if (f is null)
                throw new ArgumentNullException();

            if (f.IsZero)
                return f;

            var result = f._coefficients.Select(c => c * a).ToArray();

            return new PolynomialMonomial(result, f.VariableDomain);
        }

        public static PolynomialMonomial operator *(int a, PolynomialMonomial f)
        {
            if (f is null)
                throw new ArgumentNullException();

            return f * a;
        }

        public static PolynomialMonomial operator /(PolynomialMonomial f, PolynomialMonomial g)
        {
            if (f is null || g is null)
                throw new ArgumentNullException();

            if (!f.VariableDomain.Equals(g.VariableDomain))
                throw new PolynomialMonomialVariableNameException(f, g);

            if (g.IsZero)
                throw new DivideByZeroException();
            if (f.IsZero)
                return f;

            return DivisionWithRemainder(f, g).Item1;
        }

        public static PolynomialMonomial operator %(PolynomialMonomial f, PolynomialMonomial g)
        {
            if (f is null || g is null)
                throw new ArgumentNullException();

            if (!f.VariableDomain.Equals(g.VariableDomain))
                throw new PolynomialMonomialVariableNameException(f, g);

            if (g.IsZero)
                throw new DivideByZeroException();
            if (f.IsZero)
                return f;

            return DivisionWithRemainder(f, g).Item2;
        }

        private static (PolynomialMonomial, PolynomialMonomial) DivisionWithRemainder(PolynomialMonomial f,
            PolynomialMonomial g)
        {
            var resultLength = Math.Max(f.Degree - g.Degree + 1, 0);
            var result = new RationalMonomialsNumber[resultLength];

            var fCoefficients = (RationalMonomialsNumber[]) f._coefficients.Clone();
            var leadingG = g[g.Degree];

            for (var d1 = f.Degree; d1 >= g.Degree; --d1)
            {
                if (fCoefficients[d1].IsZero)
                    continue;

                var newCoefficient = fCoefficients[d1] / leadingG;
                var monomDegree = d1 - g.Degree;
                result[monomDegree] = newCoefficient;

                for (var d2 = 0; d2 <= g.Degree; ++d2)
                    fCoefficients[monomDegree + d2] -= newCoefficient * g[d2];
            }

            var q = new PolynomialMonomial(result, f.VariableDomain);

            var r = new PolynomialMonomial(fCoefficients, f.VariableDomain);

            return (q, r);
        }

        public static bool operator ==(PolynomialMonomial f, PolynomialMonomial g)
        {
            if (f is null || g is null)
                throw new ArgumentNullException();

            if (!f.VariableDomain.Equals(g.VariableDomain))
                throw new PolynomialMonomialVariableNameException(f, g);

            if (ReferenceEquals(f, g))
                return true;

            if (f.GetHashCode() != g.GetHashCode() || f.Degree != g.Degree)
                return false;

            for (var d = 0; d < f.Degree; d++)
                if (!f[d].Equals(g[d]))
                    return false;

            return true;
        }

        public static bool operator !=(PolynomialMonomial f, PolynomialMonomial g)
        {
            return !(f == g);
        }

        public PolynomialMonomial GetDerivative()
        {
            if (IsZero)
                return this;

            var result = new RationalMonomialsNumber[Degree];
            for (var d = 1; d <= Degree; d++)
                result[d - 1] = this[d] * d;

            return new PolynomialMonomial(result, VariableDomain);
        }

        public PolynomialMonomial Pow(BigInteger degree)
        {
            if (degree < 0)
                throw new ArgumentOutOfRangeException(nameof(degree));
            var result = new PolynomialMonomial(new List<RationalMonomialsNumber> {1}, VariableDomain);
            var a = this;

            while (degree != 0)
            {
                if (degree % 2 == 1)
                    result *= a;
                degree /= 2;
                a *= a;
            }

            return result;
        }

        public bool Equals(PolynomialMonomial other)
        {
            if (other is null)
                return false;

            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is PolynomialMonomial polynomial && Equals(polynomial);
        }

        public override int GetHashCode() => _hashCode;

        private int CalcHashCode()
        {
            var res = 0;
            foreach (var coefficient in _coefficients)
                res = HashCode.Combine(coefficient, res);

            return HashCode.Combine(res, VariableDomain, Degree);
        }
    }
}