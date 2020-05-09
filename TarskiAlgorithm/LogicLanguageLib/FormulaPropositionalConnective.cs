using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicLanguageLib
{
    public class FormulaPropositionalConnective : Formula, IEquatable<FormulaPropositionalConnective>
    {
        public readonly PropositionalConnective Connective;

        private readonly Formula[] _formulas;

        public IEnumerable<Formula> SubFormulas
        {
            get
            {
                foreach (var formula in _formulas)
                    yield return formula;
            }
        }

        public FormulaPropositionalConnective(PropositionalConnective connective, params Formula[] formulas)
        {
            Connective = connective ?? throw new ArgumentNullException(nameof(connective));

            if (formulas is null)
                throw new ArgumentNullException(nameof(formulas));
            if (formulas.Length != Connective.Arity)
                throw new ArgumentException();

            _formulas = formulas;
        }

        public override string ToString()
        {
            return Connective switch
            {
                UnaryPropositionalConnective _ => $"({Connective}{_formulas[0]})",
                BinaryPropositionalConnective _ => $"({_formulas[0]}{Connective}{_formulas[1]})",
                _ => throw new NotImplementedException()
            };
        }

        public override IEnumerable<ObjectVariable> FreeObjectVariables
        {
            get { return _formulas.SelectMany(f => f.FreeObjectVariables).Distinct(); }
        }

        public bool Equals(FormulaPropositionalConnective other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!Connective.Equals(other.Connective)) return false;

            for (var i = 0; i < _formulas.Length; i++)
                if (!_formulas[i].Equals(other._formulas[i]))
                    return false;

            return true;
        }

        public override bool Equals(Formula other)
        {
            return Equals(other as FormulaPropositionalConnective);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FormulaPropositionalConnective) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = HashCode.Combine(Connective);
            foreach (var f in _formulas)
                hashCode = HashCode.Combine(hashCode, f);

            return hashCode;
        }
    }
}