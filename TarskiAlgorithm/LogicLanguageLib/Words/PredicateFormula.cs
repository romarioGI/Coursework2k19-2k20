using System;
using System.Collections.Generic;
using System.Linq;
using LogicLanguageLib.Alphabet;

namespace LogicLanguageLib.Words
{
    public class PredicateFormula : Formula, IEquatable<PredicateFormula>
    {
        public readonly Predicate Predicate;

        private readonly Term[] _terms;

        public IEnumerable<Term> Terms
        {
            get
            {
                foreach (var term in _terms)
                    yield return term;
            }
        }

        public PredicateFormula(Predicate predicate, params Term[] terms)
        {
            Predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

            if (terms is null)
                throw new ArgumentNullException(nameof(terms));
            if (terms.Length != Predicate.Arity)
                throw new ArgumentException("count of terms must be equal predicate.Arity");

            _terms = terms;
        }

        public override string ToString()
        {
            if (Predicate is ArithmeticPredicate)
                return $"({_terms[0]}{Predicate}{_terms[1]})";

            return $"{Predicate}({string.Join<Term>(',', _terms)})";
        }

        public override IEnumerable<ObjectVariable> FreeObjectVariables
        {
            get { return _terms.SelectMany(t => t.FreeObjectVariables).Distinct(); }
        }

        public bool Equals(PredicateFormula other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!Predicate.Equals(other.Predicate)) return false;

            for (var i = 0; i < _terms.Length; i++)
                if (!_terms[i].Equals(other._terms[i]))
                    return false;

            return true;
        }

        public override bool Equals(Formula other)
        {
            return Equals(other as PredicateFormula);
        }

        public override int GetHashCode()
        {
            var hashCode = HashCode.Combine(Predicate);
            foreach (var t in _terms)
                hashCode = HashCode.Combine(hashCode, t);

            return hashCode;
        }
    }
}