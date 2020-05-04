using System;
using System.Collections.Generic;
using System.Linq;
using MathLib;

namespace TarskiAlgorithmLib
{
    public static class SignsGenerator<T> where T: AbstractNumber
    {
        public static IEnumerable<List<List<Sign>>> Get(List<Polynomial<T>> polynomials)
        {
            var candidates = GetCandidates(polynomials).ToArray();
            var quantities = candidates.Select(arr => arr.Count).ToArray();

            foreach (var arrangement in GetArrangements(quantities))
            {
                var i = 0;
                var resP = new List<List<Sign>>();
                foreach (var p in polynomials)
                {
                    var resC = new List<Sign>();
                    for (var d = 0; d <= p.Degree; d++)
                    {
                        resC.Add(candidates[i][arrangement[i]]);
                        i++;
                    }

                    resP.Add(resC);
                }

                yield return resP;
            }

        }

        private static IEnumerable<List<Sign>> GetCandidates(IEnumerable<Polynomial<T>> polynomials)
        {
            return polynomials.SelectMany(p => p.Coefficients.Select(GetCandidates));
        }

        private static List<Sign> GetCandidates(AbstractNumber number)
        {
            var result = new List<Sign>();

            if (number.Sign.HasFlag(Sign.LessZero))
                result.Add(Sign.LessZero);
            if (number.Sign.HasFlag(Sign.Zero))
                result.Add(Sign.Zero);
            if (number.Sign.HasFlag(Sign.MoreZero))
                result.Add(Sign.MoreZero);

            return result;
        }

        private static IEnumerable<List<int>> GetArrangements(int[] quantities)
        {
            var count = quantities.Length;

            var totalCount = 1;
            try
            {
                for (var i = 0; i < count; i++)
                    totalCount = checked(totalCount * quantities[i]);
            }
            catch (OverflowException e)
            {
                throw new Exception("Too many options. It will take too much time.", e);
            }

            for (var i = 0; i < totalCount; i++)
                yield return ToList(i);

            List<int> ToList(int x)
            {
                var result = new List<int>(count);
                for (var i = 0; i < count; i++)
                {
                    result[i] = x % quantities[i];
                    x /= quantities[i];
                }

                return result;
            }
        }
    }
}
