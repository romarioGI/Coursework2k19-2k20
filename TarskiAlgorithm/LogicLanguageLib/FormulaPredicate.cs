using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicLanguageLib
{
    public class FormulaPredicate : Formula
    {
        public readonly AbstractPredicate Predicate;

        private readonly Term[] _terms;

        public IEnumerable<Term> Terms
        {
            get
            {
                foreach (var term in _terms)
                    yield return term;
            }
        }

        public FormulaPredicate(AbstractPredicate predicate, params Term[] terms)
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
    }
}