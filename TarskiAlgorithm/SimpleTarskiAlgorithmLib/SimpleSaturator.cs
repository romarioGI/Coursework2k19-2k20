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

            var queue = new Queue<Polynomial>();

            var system = polynomials
                .Where(p => !p.IsZero)
                .Distinct();

            foreach (var p in system)
                queue.Enqueue(p);

            if (queue.Count == 0)
                return new List<Polynomial>();

            var multiplication = queue
                .Aggregate((res, nxt) => res * nxt);
            queue.Enqueue(multiplication.GetDerivative());

            while (queue.Count != 0)
            {
                var cur = queue.Dequeue();
                if (cur.IsZero || result.Contains(cur))
                    continue;

                queue.Enqueue(cur.GetDerivative());
                foreach (var r in GetRemainders(result, cur))
                    queue.Enqueue(r);

                result.Add(cur);
            }


            return result;
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