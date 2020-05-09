using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicLanguageLib
{
    public class FormulaPredicate : Formula, IEquatable<FormulaPredicate>
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

        public FormulaPredicate(Predicate predicate, params Term[] terms)
        {
            Predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

            if (terms is null)
                throw new ArgumentNullException(nameof(terms));
            if (terms.Length != Predicate.Arity)
                throw new ArgumentException();

            _terms = terms;
        }

        public override string ToString()
        {
            return $"{Predicate}({string.Join<Term>(',', _terms)})";
        }

        public override IEnumerable<ObjectVariable> FreeObjectVariables
        {
            get { return _terms.SelectMany(t => t.FreeObjectVariables).Distinct(); }
        }

        public bool Equals(FormulaPredicate other)
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
            return Equals(other as FormulaPredicate);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FormulaPredicate) obj);
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