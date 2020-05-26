using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTarskiAlgorithmLib;

namespace TarskiAlgorithmLib
{
    public struct Monomial : IEquatable<Monomial>
    {
        private readonly (VariableName, int)[] _variableDegree;

        public static Monomial EmptyMonomial => new Monomial(new VariableName[] { });

        public Monomial(params VariableName[] variables)
            : this(variables.Select(x => (x, 1)))
        {

        }

        public Monomial(params (VariableName, int)[] variables)
            : this(variables.Select(x => x))
        {

        }

        public Monomial(IEnumerable<(VariableName, int)> variables)
        {
            var dict = new Dictionary<VariableName, int>();
            foreach (var (variableName, degree) in variables)
            {
                if (degree < 0)
                    throw new ArgumentOutOfRangeException(nameof(degree), "degree must be greater than zero");
                if (degree == 0)
                    continue;
                if (dict.ContainsKey(variableName))
                    dict[variableName] += degree;
                else
                    dict.Add(variableName, degree);
            }

            _variableDegree = dict
                .Select(p => (p.Key, p.Value))
                .OrderBy(p => p.Key)
                .ToArray();
        }

        public bool Equals(Monomial other)
        {
            return IsEquals(_variableDegree, other._variableDegree);
        }

        private static bool IsEquals((VariableName, int)[] first, (VariableName, int)[] second)
        {
            if (first.Length != second.Length)
                return false;

            return !first.Where((t, i) => !t.Equals(second[i])).Any();
        }

        public override bool Equals(object obj)
        {
            return obj is Monomial monomial && Equals(monomial);
        }

        public override int GetHashCode()
        {
            var res = _variableDegree.Length;
            foreach (var (variable, degree) in _variableDegree)
                res = HashCode.Combine(res, variable, degree);

            return res;
        }

        //TODO test
        public static Monomial operator *(Monomial first, Monomial second)
        {
            var res = first._variableDegree.Concat(second._variableDegree);

            return new Monomial(res);
        }

        public override string ToString()
        {
            return string.Join("", _variableDegree.Select(p => $"{p.Item1}^{p.Item2}"));
        }
    }
}