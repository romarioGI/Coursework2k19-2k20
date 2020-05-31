using System;
using System.Collections.Generic;
using System.Linq;
using LogicLanguageLib.Alphabet;

namespace LogicLanguageLib.Words
{
    public class FunctionTerm : Term, IEquatable<FunctionTerm>
    {
        public readonly Function Function;

        private readonly Term[] _terms;

        public IEnumerable<Term> Terms
        {
            get
            {
                foreach (var term in _terms)
                    yield return term;
            }
        }

        public FunctionTerm(Function function, params Term[] terms)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));

            if (terms is null)
                throw new ArgumentNullException(nameof(terms));
            if (terms.Length != Function.Arity)
                throw new ArgumentException("count of terms must be equal function.Arity");

            _terms = terms;
        }

        public override string ToString()
        {
            if (Function is ArithmeticBinaryFunction)
                return $"{LeftBracket.GetInstance()}{_terms[0]}{Function}{_terms[1]}{RightBracket.GetInstance()}";
            return $"{Function}{LeftBracket.GetInstance()}{string.Join<Term>(Comma.GetInstance().ToString(), _terms)}{RightBracket.GetInstance()}";
        }

        public override IEnumerable<ObjectVariable> FreeObjectVariables
        {
            get { return _terms.SelectMany(t => t.FreeObjectVariables).Distinct(); }
        }

        public override bool Equals(Term other)
        {
            return Equals(other as FunctionTerm);
        }

        public override int GetHashCode()
        {
            var hashCode = Function.GetHashCode();
            foreach (var t in _terms)
                HashCode.Combine(hashCode, t.GetHashCode());

            return hashCode;
        }

        public bool Equals(FunctionTerm other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!Function.Equals(other.Function)) return false;

            for (var i = 0; i < _terms.Length; i++)
                if (!_terms[i].Equals(other._terms[i]))
                    return false;

            return true;
        }
    }
}