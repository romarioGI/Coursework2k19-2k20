using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicLanguageLib
{
    public class FormulaPropositionalConnective : Formula
    {
        public readonly PropositionalConnective Connective;

        private readonly Formula[] _formulas;

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
    }
}