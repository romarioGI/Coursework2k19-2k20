using System.Collections.Generic;
using System.Linq;

namespace SimpleTarskiAlgorithmLib
{
    /// <summary>
    /// saturator of a polynomial system
    /// </summary>
    public static class SimpleSaturator
    {
        public static IEnumerable<Polynomial> Saturate(IEnumerable<Polynomial> polynomials)
        {
            var result = new HashSet<Polynomial>();

            var system = polynomials
                .Where(p => !p.IsZero)
                .Distinct()
                .ToList();

            if (system.Count == 0)
                return system;

            var multiplication = system
                .Aggregate((res, nxt) => res * nxt);
            system.Add(multiplication.GetDerivative());

            foreach (var p in system)
                Add(result, p);

            return result;
        }

        private static void Add(HashSet<Polynomial> system, Polynomial polynomial)
        {
            if (polynomial.IsZero)
                return;

            if (system.Contains(polynomial))
                return;

            Add(system, polynomial.GetDerivative());

            // важно привести к массиву, так как в этом методе используется system, поэтому его нельзя менять, пока метод не завершиться
            var remainders = GetRemainders(system, polynomial).ToArray();

            system.Add(polynomial);

            foreach (var p in remainders)
                Add(system, p);
        }

        private static IEnumerable<Polynomial> GetRemainders(IEnumerable<Polynomial> system, Polynomial polynomial)
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