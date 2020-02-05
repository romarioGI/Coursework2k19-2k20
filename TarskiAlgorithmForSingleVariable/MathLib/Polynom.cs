using System;

namespace MathLib
{
    //TODO написать тесты
    public class Polynom : IEquatable<Polynom>, IComparable<Polynom>
    {
        private readonly RationalNumber[] _coefficients;
        private readonly int _hashCode;

        //TODO test Zero Polynom
        public Polynom(RationalNumber[] coefficients)
        {
            var degree = coefficients.Length - 1;
            while (degree >= 0 && _coefficients[degree].IsZero())
                --degree;

            _coefficients = new RationalNumber[degree + 1];
            for (var i = 0; i <= degree; ++i)
                _coefficients[i] = coefficients[i];

            Degree = degree;
            _hashCode = CalcHashCode();
        }

        public readonly int Degree;

        public bool IsZero => Degree == -1;

        public RationalNumber this[int degree]
        {
            get
            {
                if (degree < 0)
                    throw new ArgumentOutOfRangeException();

                return degree < _coefficients.Length ? _coefficients[degree] : 0;
            }
        }

        public static Polynom operator +(Polynom f, Polynom g)
        {
            var result = new RationalNumber[Math.Max(f.Degree, g.Degree) + 1];
            for (var d = 0; d < result.Length; ++d)
                result[d] = f[d] + g[d];

            return new Polynom(result);
        }

        public static Polynom operator -(Polynom f, Polynom g)
        {
            var result = new RationalNumber[Math.Max(f.Degree, g.Degree) + 1];
            for (var d = 0; d < result.Length; ++d)
                result[d] = f[d] - g[d];

            return new Polynom(result);
        }

        public static Polynom operator *(Polynom f, Polynom g)
        {
            var result = new RationalNumber[f.Degree + g.Degree + 1];
            for (var d1 = 0; d1 < result.Length; ++d1)
            for (var d2 = 0; d2 < result.Length; ++d2)
                result[d1 + d2] += f[d1] * g[d2];

            return new Polynom(result);
        }

        public static Polynom operator /(Polynom f, Polynom g)
        {
            if (g.IsZero)
                throw new DivideByZeroException();

            return DivisionWithRemainder(f, g).Item1;
        }

        public static Polynom operator %(Polynom f, Polynom g)
        {
            if (g.IsZero)
                throw new DivideByZeroException();

            return DivisionWithRemainder(f, g).Item2;
        }

        private static (Polynom, Polynom) DivisionWithRemainder(Polynom f, Polynom g)
        {
            var result = new RationalNumber[f.Degree - g.Degree + 1];
            var fCoefficients = (RationalNumber[]) f._coefficients.Clone();
            var leadingG = g[g.Degree];

            for (var d1 = f.Degree; d1 >= g.Degree; --d1)
            {
                if (fCoefficients[d1].IsZero())
                    continue;

                var newCoefficient = fCoefficients[d1] / leadingG;
                var monomDegree = d1 - g.Degree;
                result[monomDegree] = newCoefficient;

                for (var d2 = 0; d2 < g.Degree; ++d2)
                    fCoefficients[monomDegree + d2] -= newCoefficient * g[d2];

                fCoefficients[d1] = 0;
            }

            return (new Polynom(result), new Polynom(fCoefficients));
        }

        public static bool operator ==(Polynom f, Polynom g)
        {
            if (f is null || g is null)
                throw new ArgumentNullException();

            if (ReferenceEquals(f, g))
                return true;

            if (f.GetHashCode() != g.GetHashCode() || f.Degree != g.Degree)
                return false;

            for (var d = 0; d < f.Degree; d++)
                if (f[d] != g[d])
                    return false;

            return true;
        }

        public static bool operator !=(Polynom f, Polynom g)
        {
            return !(f == g);
        }

        public static implicit operator Polynom(RationalNumber num)
        {
            return new Polynom(new[] {num});
        }

        public Polynom GetDerivative()
        {
            var result = new RationalNumber[Degree];
            for (var d = 1; d <= Degree; d++)
                result[d - 1] = this[d] * d;

            return new Polynom(result);
        }

        public bool Equals(Polynom other)
        {
            if (other is null)
                return false;

            return this == other;
        }

        public override bool Equals(object obj) => Equals(obj as Polynom);

        public override int GetHashCode() => _hashCode;

        private int CalcHashCode()
        {
            const int p = 53;

            var res = 0;
            foreach (var x in _coefficients)
                res = res * p + x.GetHashCode();

            return res ^ Degree;
        }

        public int CompareTo(Polynom other)
        {
            if (other is null)
                throw new ArgumentNullException();
            if (ReferenceEquals(this, other))
                return 0;

            var cmpRes = Degree.CompareTo(other.Degree);
            if (cmpRes != 0)
                return cmpRes;

            for (var d = Degree; d >= 0; --d)
            {
                cmpRes = this[d].CompareTo(other[d]);
                if (cmpRes != 0)
                    return cmpRes;
            }

            return 0;
        }
    }
}