﻿using System.Collections.Generic;
using System.Linq;
using MathLib;

//TODO написать тесты

namespace TarskiAlgorithmLib
{
    /// <summary>
    /// saturator of a polynomial system
    /// </summary>
    public static class Saturator
    {
        public static IEnumerable<Polynomial<T>> Saturate<T>(IEnumerable<Polynomial<T>> polynomials)
            where T : AbstractNumber
        {
            var result = new HashSet<Polynomial<T>>();

            var system = polynomials
                .Where(p => !p.IsZero)
                .OrderBy(p => p, new PolynomialsDegreeComparator<T>())
                .ToList();

            var multiplication = system
                .Aggregate((res, nxt) => res * nxt);
            system.Add(multiplication.GetDerivative());

            foreach (var p in system)
                Add(result, p);

            return result.OrderBy(p => p, new PolynomialsDegreeComparator<T>());
        }

        private static void Add<T>(HashSet<Polynomial<T>> system, Polynomial<T> polynomial)
            where T : AbstractNumber
        {
            if (polynomial.IsZero)
                return;

            if (system.Contains(polynomial))
                return;

            Add(system, polynomial.GetDerivative());

            // важно привести к массиву, так как в этом методе используется system, поэтому его нельзя менять, пока метод не завершиться
            var remainders = GetRemainders(system, polynomial)
                .Where(p => !p.IsZero).ToArray();

            system.Add(polynomial);

            foreach (var p in remainders)
                Add(system, p);
        }

        private static IEnumerable<Polynomial<T>> GetRemainders<T>(
            IEnumerable<Polynomial<T>> system,
            Polynomial<T> polynomial)
            where T : AbstractNumber
        {
            foreach (var p in system)
            {
                if (p.Degree <= polynomial.Degree)
                    yield return polynomial % p;

                if (polynomial.Degree <= p.Degree)
                    yield return p % polynomial;
            }
        }
    }
}