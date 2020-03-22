using System;

namespace MathLib
{
    //TODO написать тесты
    public class Polynom<T> : IEquatable<Polynom<T>> where T:INumber
    {
        private readonly Arithmetic<T>[] _coefficients;
        private readonly int _hashCode;

        //TODO test Zero Polynom
        public Polynom(T[] coefficients)
        {
            var degree = coefficients.Length - 1;
            while (degree >= 0 && coefficients[degree].IsZero)
                --degree;

            _coefficients = new Arithmetic<T>[degree + 1];
            for (var i = 0; i <= degree; ++i)
                _coefficients[i] = coefficients[i];

            Degree = degree;
            _hashCode = CalcHashCode();
        }

        private Polynom(Arithmetic<T>[] coefficients)
        {
            throw new NotImplementedException();
        }

        public readonly int Degree;

        public bool IsZero => Degree == -1;

        public Arithmetic<T> this[int degree]
        {
            get
            {
                if (degree < 0)
                    throw new ArgumentOutOfRangeException();

                return degree < _coefficients.Length ? _coefficients[degree] : Arithmetic<T>.Zero;
            }
        }

        public static Polynom<T> operator +(Polynom<T> f, Polynom<T> g)
        {
            var result = new Arithmetic<T>[Math.Max(f.Degree, g.Degree) + 1];
            for (var d = 0; d < result.Length; ++d)
                result[d] = f[d] + g[d];

            return new Polynom<T>(result);
        }

        public static Polynom<T> operator -(Polynom<T> f, Polynom<T> g)
        {
            var result = new Arithmetic<T>[Math.Max(f.Degree, g.Degree) + 1];
            for (var d = 0; d < result.Length; ++d)
                result[d] = f[d] - g[d];

            return new Polynom<T>(result);
        }

        public static Polynom<T> operator *(Polynom<T> f, Polynom<T> g)
        {
            var result = new Arithmetic<T>[f.Degree + g.Degree + 1];
            for (var d1 = 0; d1 < result.Length; ++d1)
            for (var d2 = 0; d2 < result.Length; ++d2)
                result[d1 + d2] += f[d1] * g[d2];

            return new Polynom<T>(result);
        }

        public static Polynom<T> operator /(Polynom<T> f, Polynom<T> g)
        {
            if (g.IsZero)
                throw new DivideByZeroException();

            return DivisionWithRemainder(f, g).Item1;
        }

        public static Polynom<T> operator %(Polynom<T> f, Polynom<T> g)
        {
            if (g.IsZero)
                throw new DivideByZeroException();

            return DivisionWithRemainder(f, g).Item2;
        }

        public (Polynom<T>, Polynom<T>) DivisionWithRemainder(Polynom<T> g)
        {
            return DivisionWithRemainder(this, g);
        }

        private static (Polynom<T>, Polynom<T>) DivisionWithRemainder(Polynom<T> f, Polynom<T> g)
        {
            var result = new Arithmetic<T>[f.Degree - g.Degree + 1];
            var fCoefficients = (Arithmetic<T>[]) f._coefficients.Clone();
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

                fCoefficients[d1] = Arithmetic<T>.Zero;
            }

            return (new Polynom<T>(result), new Polynom<T>(fCoefficients));
        }

        public static bool operator ==(Polynom<T> f, Polynom<T> g)
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

        public static bool operator !=(Polynom<T> f, Polynom<T> g)
        {
            return !(f == g);
        }

        public static implicit operator Polynom<T>(Arithmetic<T> num)
        {
            return new Polynom<T>(new[] {num});
        }

        public Polynom<T> GetDerivative()
        {
            var result = new Arithmetic<T>[Degree];
            for (var d = 1; d <= Degree; d++)
                result[d - 1] = this[d] * d;

            return new Polynom<T>(result);
        }

        public bool Equals(Polynom<T> other)
        {
            if (other is null)
                return false;

            return this == other;
        }

        public override bool Equals(object obj) => Equals(obj as Polynom<T>);

        public override int GetHashCode() => _hashCode;

        //TODO
        private int CalcHashCode()
        {
            var res = HashCode.Combine(Degree);
            foreach (var coefficient in _coefficients)
                res = HashCode.Combine(coefficient, res);

            return res ^ Degree;
        }
    }
}