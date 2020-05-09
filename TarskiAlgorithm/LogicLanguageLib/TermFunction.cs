using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicLanguageLib
{
    public class TermFunction<TOut> : Term
    {
        public readonly AbstractFunction<TOut> Function;

        private readonly Term[] _terms;

        public IEnumerable<Term> Terms
        {
            get
            {
                foreach (var term in _terms)
                    yield return term;
            }
        }

        public TermFunction(AbstractFunction<TOut> function, params Term[] terms)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));

            if (terms is null)
                throw new ArgumentNullException(nameof(terms));
            if (terms.Length != Function.Arity)
                throw new ArgumentException();

            _terms = terms;
        }

        public override string ToString()
        {
            return $"{Function}({string.Join<Term>(',', _terms)})";
        }

        public override IEnumerable<ObjectVariable> FreeObjectVariables
        {
            get { return _terms.SelectMany(t => t.FreeObjectVariables).Distinct(); }
        }
    }
}