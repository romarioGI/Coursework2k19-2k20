using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MathLib.Exceptions;

namespace MathLib
{
    public class Polynomial<T> : AbstractNumber where T : AbstractNumber
    {
        private readonly AbstractNumber[] _coefficients;
        private readonly int _hashCode;

        public readonly VariableDomain VariableDomain;

        public Polynomial(IEnumerable<T> coefficients, VariableDomain variableDomain) :
            this(coefficients.Select(c => c as AbstractNumber).ToArray(), variableDomain)
        {
        }

        private Polynomial(AbstractNumber[] coefficients, VariableDomain variableDomain,
            Sign preSetSign = Sign.Undefined)
        {
            VariableDomain = variableDomain;

            var degree = coefficients.Length - 1;
            while (degree >= 0 && (coefficients[degree] is null || coefficients[degree].IsZero))
                --degree;

            var canZero = true;
            var canNotZero = false;

            _coefficients = new AbstractNumber[degree + 1];
            for (var i = 0; i <= degree; ++i)
            {
                _coefficients[i] = coefficients[i];
                canZero &= _coefficients[i].CanZero;
                canNotZero |= !_coefficients[i].IsZero;
            }

            Degree = degree;
            _hashCode = CalcHashCode();

            if (Degree == 0)
                preSetSign = _coefficients[0].Sign;

            Sign = CalcSign(preSetSign, canZero, canNotZero);
        }

        private static Sign CalcSign(Sign preSetSign, bool canZero, bool canNotZero)
        {
            var sign = preSetSign;

            if (!canNotZero)
                sign &= Sign.Zero;

            if (!canZero)
                sign &= Sign.NotZero;

            return sign;
        }

        public readonly int Degree;

        private AbstractNumber this[int degree]
        {
            get
            {
                if (degree < 0 || degree > Degree)
                    throw new ArgumentOutOfRangeException();

                return _coefficients[degree];
            }
        }

        public IEnumerable<AbstractNumber> Coefficients => _coefficients.Select(c => c);

        public static Polynomial<T> operator +([NotNull] Polynomial<T> f, [NotNull] Polynomial<T> g)
        {
            if (!f.VariableDomain.Equals(g.VariableDomain))
                throw new PolynomialObjectVariableException<T>(f, g);

            var result = new AbstractNumber[Math.Max(f.Degree, g.Degree) + 1];
            var minDegree = Math.Min(f.Degree, g.Degree);
            for (var d = 0; d <= minDegree; ++d)
                result[d] = f[d] + g[d];

            for (var d = minDegree + 1; d <= f.Degree; ++d)
                result[d] = f[d];
            for (var d = minDegree + 1; d <= g.Degree; ++d)
                result[d] = g[d];

            return new Polynomial<T>(result, f.VariableDomain, f.Sign.Add(g.Sign));
        }

        public static Polynomial<T> operator -([NotNull]Polynomial<T> f, [NotNull]Polynomial<T> g)
        {
            if (!f.VariableDomain.Equals(g.VariableDomain))
                throw new PolynomialObjectVariableException<T>(f, g);

            var result = new AbstractNumber[Math.Max(f.Degree, g.Degree) + 1];
            var minDegree = Math.Min(f.Degree, g.Degree);
            for (var d = 0; d <= minDegree; ++d)
                result[d] = f[d] - g[d];

            for (var d = minDegree + 1; d <= f.Degree; ++d)
                result[d] = f[d];
            for (var d = minDegree + 1; d <= g.Degree; ++d)
                result[d] = -g[d];

            return new Polynomial<T>(result, f.VariableDomain, f.Sign.Add(g.Sign.Invert()));
        }

        public static Polynomial<T> operator *([NotNull] Polynomial<T> f, [NotNull] Polynomial<T> g)
        {
            if (!f.VariableDomain.Equals(g.VariableDomain))
                throw new PolynomialObjectVariableException<T>(f, g);

            var result = new AbstractNumber[f.Degree + g.Degree + 1];
            for (var d1 = 0; d1 <= f.Degree; ++d1)
            for (var d2 = 0; d2 <= g.Degree; ++d2)
                result[d1 + d2] += f[d1] * g[d2];

            return new Polynomial<T>(result, f.VariableDomain, f.Sign.Multi(g.Sign));
        }

        public static Polynomial<T> operator *([NotNull] Polynomial<T> f, int a)
        {
            var result = f._coefficients.Select(c => c * a).ToArray();

            Sign signA;
            if (a < 0)
                signA = Sign.LessZero;
            else if (a > 0)
                signA = Sign.MoreZero;
            else
                signA = Sign.Zero;

            return new Polynomial<T>(result, f.VariableDomain, f.Sign.Multi(signA));
        }

        public static Polynomial<T> operator *(int a, [NotNull] Polynomial<T> f)
        {
            return f * a;
        }

        public static Polynomial<T> operator /([NotNull]Polynomial<T> f, [NotNull]Polynomial<T> g)
        {
            if (!f.VariableDomain.Equals(g.VariableDomain))
                throw new PolynomialObjectVariableException<T>(f, g);

            if (g.IsZero)
                throw new DivideByZeroException();

            return DivisionWithRemainder(f, g).Item1;
        }

        public static Polynomial<T> operator %([NotNull]Polynomial<T> f, [NotNull]Polynomial<T> g)
        {
            if (!f.VariableDomain.Equals(g.VariableDomain))
                throw new PolynomialObjectVariableException<T>(f, g);

            if (g.IsZero)
                throw new DivideByZeroException();

            return DivisionWithRemainder(f, g).Item2;
        }

        public (Polynomial<T>, Polynomial<T>) DivisionWithRemainder([NotNull]Polynomial<T> g)
        {
            if (!VariableDomain.Equals(g.VariableDomain))
                throw new PolynomialObjectVariableException<T>(this, g);

            return DivisionWithRemainder(this, g);
        }

        private static (Polynomial<T>, Polynomial<T>) DivisionWithRemainder([NotNull] Polynomial<T> f,
            [NotNull] Polynomial<T> g)
        {
            var resultLength = Math.Max(f.Degree - g.Degree + 1, 0);
            var result = new AbstractNumber[resultLength];

            var fCoefficients = (AbstractNumber[]) f._coefficients.Clone();
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

            var q = new Polynomial<T>(result, f.VariableDomain);

            var rPresetSign = f.Sign.Add(g.Sign.Multi(q.Sign).Invert());
            var r = new Polynomial<T>(fCoefficients, f.VariableDomain, rPresetSign);

            return (q, r);
        }

        public static bool operator ==(Polynomial<T> f, Polynomial<T> g)
        {
            if (f is null || g is null)
                throw new ArgumentNullException();

            if (!f.VariableDomain.Equals(g.VariableDomain))
                throw new PolynomialObjectVariableException<T>(f, g);

            if (ReferenceEquals(f, g))
                return true;

            if (f.GetHashCode() != g.GetHashCode() || f.Degree != g.Degree)
                return false;

            for (var d = 0; d < f.Degree; d++)
                if (!f[d].Equals(g[d]))
                    return false;

            return true;
        }

        public static bool operator !=(Polynomial<T> f, Polynomial<T> g)
        {
            return !(f == g);
        }

        public Polynomial<T> GetDerivative()
        {
            if (IsZero)
                return this;

            var result = new AbstractNumber[Degree];
            for (var d = 1; d <= Degree; d++)
                result[d - 1] = this[d] * d;

            return new Polynomial<T>(result, VariableDomain);
        }

        public bool Equals(Polynomial<T> other)
        {
            if (other is null)
                return false;

            return this == other;
        }

        protected override bool EqualsNotZeroAndSameType(AbstractNumber other)
        {
            return Equals((Polynomial<T>) other);
        }

        public override int GetHashCode() => _hashCode;

        protected override AbstractNumber SetVerifiedSign(Sign sign)
        {
            return new Polynomial<T>(_coefficients, VariableDomain) {Sign = sign};
        }

        public AbstractNumber SetCoefficientsSigns(List<Sign> signs)
        {
            var newCoefficients = _coefficients.Select((c, i) => c.SetSign(signs[i]));

            return new Polynomial<T>(newCoefficients.ToArray(), VariableDomain, Sign);
        }

        private int CalcHashCode()
        {
            var res = 0;
            foreach (var coefficient in _coefficients)
                res = HashCode.Combine(coefficient, res);

            return HashCode.Combine(res, VariableDomain, Degree);
        }

        protected override AbstractNumber AddNotZeroAndSameTypes(AbstractNumber abstractNumber)
        {
            return this + (Polynomial<T>) abstractNumber;
        }

        protected override AbstractNumber SubtractNotZeroAndSameTypes(AbstractNumber abstractNumber)
        {
            return this - (Polynomial<T>) abstractNumber;
        }

        protected override AbstractNumber GetOppositeNotZero()
        {
            var result = new AbstractNumber[Degree];
            for (var d = 0; d <= Degree; d++)
                result[d] = -this[d];

            return new Polynomial<T>(result, VariableDomain);
        }

        protected override AbstractNumber MultiplyNotZeroAndSameTypes(AbstractNumber abstractNumber)
        {
            return this * (Polynomial<T>) abstractNumber;
        }

        protected override AbstractNumber Multiply(int number)
        {
            return this * number;
        }

        protected override AbstractNumber DivideNotZeroAndSameTypes(AbstractNumber abstractNumber)
        {
            return this / (Polynomial<T>) abstractNumber;
        }

        protected override AbstractNumber GetRemainderNotZeroAndSameTypes(AbstractNumber abstractNumber)
        {
            return this % (Polynomial<T>) abstractNumber;
        }
    }
}