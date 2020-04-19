using System;
using System.Collections.Generic;

namespace MathLib
{
    public class PolynomialsDegreeComparator<T> : IComparer<Polynomial<T>> where T : AbstractNumber
    {
        public int Compare(Polynomial<T> x, Polynomial<T> y)
        {
            if(x is null || y is null)
                throw new ArgumentNullException();

            return x.Degree.CompareTo(y.Degree);
        }
    }
}