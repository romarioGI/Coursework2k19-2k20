using System;
using System.Collections.Generic;
using System.Linq;
using MathLib.Exceptions;

namespace MathLib
{
    public class Polynomial<T> : AbstractNumber  where T : AbstractNumber
    {
        private readonly AbstractNumber[] _coefficients;
        private readonly int _hashCode;

        public readonly ObjectVariable ObjectVariable;

        public Polynomial(IEnumerable<T> coefficients, ObjectVariable objectVariable): 
            this(coefficients.Select(c => (AbstractNumber)c).ToArray(), objectVariable)
        {
            
        }

        private Polynomial(AbstractNumber[] coefficients, ObjectVariable objectVariable)
        {
            ObjectVariable = objectVariable;

            var degree = coefficients.Length - 1;
            while (degree >= 0 && coefficients[degree].IsZero)
                --degree;

            _coefficients = new AbstractNumber[degree + 1];
            for (var i = 0; i <= degree; ++i)
                _coefficients[i] = coefficients[i];

            Degree = degree;
            _hashCode = CalcHashCode();

            Sign = Degree == -1 ? Sign.Zero : Sign.NotZero;
        }

        public readonly int Degree;

        private AbstractNumber this[int degree]
        {
            get
            {
                if (degree < 0)
                    throw new ArgumentOutOfRangeException();

                return degree < _coefficients.Length ? _coefficients[degree] : Zero;
            }
        }

        public static Polynomial<T> operator +(Polynomial<T> f, Polynomial<T> g)
        {
            if (!f.ObjectVariable.Equals(g.ObjectVariable))
                throw new PolynomialObjectVariableException<T>(f, g);

            var result = new AbstractNumber[Math.Max(f.Degree, g.Degree) + 1];
            for (var d = 0; d < result.Length; ++d)
                result[d] = f[d] + g[d];

            return new Polynomial<T>(result, f.ObjectVariable);
        }

        public static Polynomial<T> operator -(Polynomial<T> f, Polynomial<T> g)
        {
            if (!f.ObjectVariable.Equals(g.ObjectVariable))
                throw new PolynomialObjectVariableException<T>(f, g);

            var result = new AbstractNumber[Math.Max(f.Degree, g.Degree) + 1];
            for (var d = 0; d < result.Length; ++d)
                result[d] = f[d] - g[d];

            return new Polynomial<T>(result, f.ObjectVariable);
        }

        public static Polynomial<T> operator *(Polynomial<T> f, Polynomial<T> g)
        {
            if (!f.ObjectVariable.Equals(g.ObjectVariable))
                throw new PolynomialObjectVariableException<T>(f, g);

            var result = new AbstractNumber[f.Degree + g.Degree + 1];
            for (var d1 = 0; d1 < result.Length; ++d1)
            for (var d2 = 0; d2 < result.Length; ++d2)
                result[d1 + d2] += f[d1] * g[d2];

            return new Polynomial<T>(result, f.ObjectVariable);
        }

        public static Polynomial<T> operator /(Polynomial<T> f, Polynomial<T> g)
        {
            if (!f.ObjectVariable.Equals(g.ObjectVariable))
                throw new PolynomialObjectVariableException<T>(f, g);

            if (g.IsZero)
                throw new DivideByZeroException();

            return DivisionWithRemainder(f, g).Item1;
        }

        public static Polynomial<T> operator %(Polynomial<T> f, Polynomial<T> g)
        {
            if (!f.ObjectVariable.Equals(g.ObjectVariable))
                throw new PolynomialObjectVariableException<T>(f, g);

            if (g.IsZero)
                throw new DivideByZeroException();

            return DivisionWithRemainder(f, g).Item2;
        }

        public (Polynomial<T>, Polynomial<T>) DivisionWithRemainder(Polynomial<T> g)
        {
            if (!this.ObjectVariable.Equals(g.ObjectVariable))
                throw new PolynomialObjectVariableException<T>(this, g);

            return DivisionWithRemainder(this, g);
        }

        private static (Polynomial<T>, Polynomial<T>) DivisionWithRemainder(Polynomial<T> f, Polynomial<T> g)
        {
            var result = new AbstractNumber[f.Degree - g.Degree + 1];
            var fCoefficients = (AbstractNumber[]) f._coefficients.Clone();
            var leadingG = g[g.Degree];

            for (var d1 = f.Degree; d1 >= g.Degree; --d1)
            {
                if (fCoefficients[d1].IsZero)
                    continue;

                var newCoefficient = fCoefficients[d1] / leadingG;
                var monomDegree = d1 - g.Degree;
                result[monomDegree] = newCoefficient;

                for (var d2 = 0; d2 < g.Degree; ++d2)
                    fCoefficients[monomDegree + d2] -= newCoefficient * g[d2];

                fCoefficients[d1] = Zero;
            }

            return (new Polynomial<T>(result, f.ObjectVariable), new Polynomial<T>(fCoefficients, f.ObjectVariable));
        }

        public static bool operator ==(Polynomial<T> f, Polynomial<T> g)
        {
            if (f is null || g is null)
                throw new ArgumentNullException();

            if (!f.ObjectVariable.Equals(g.ObjectVariable))
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
            var result = new AbstractNumber[Degree];
            for (var d = 1; d <= Degree; d++)
                result[d - 1] = this[d] * d;

            return new Polynomial<T>(result, ObjectVariable);
        }

        public bool Equals(Polynomial<T> other)
        {
            if (other is null)
                return false;

            return this == other;
        }

        protected override bool EqualsNotZeroAndEqualType(AbstractNumber other)
        {
            return Equals((Polynomial<T>) other);
        }

        public override int GetHashCode() => _hashCode;

        private int CalcHashCode()
        {
            var res = HashCode.Combine(Degree);
            foreach (var coefficient in _coefficients)
                res = HashCode.Combine(coefficient, res);

            return res ^ Degree;
        }

        public override Sign Sign { get; }

        protected override AbstractNumber AddNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return this + (Polynomial<T>) abstractNumber;
        }

        protected override AbstractNumber SubtractNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return this - (Polynomial<T>) abstractNumber;
        }

        protected override AbstractNumber GetOpposite()
        {
            var result = new AbstractNumber[Degree];
            for (var d = 0; d <= Degree; d++)
                result[d] = -this[d];

            return new Polynomial<T>(result, ObjectVariable);
        }

        protected override AbstractNumber MultiplyNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return this * (Polynomial<T>) abstractNumber;
        }

        protected override AbstractNumber DivideNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return this / (Polynomial<T>) abstractNumber;
        }

        protected override AbstractNumber GetRemainderNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return this % (Polynomial<T>) abstractNumber;
        }
    }
}