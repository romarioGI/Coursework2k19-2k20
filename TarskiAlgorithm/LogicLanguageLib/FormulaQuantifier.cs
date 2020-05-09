using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicLanguageLib
{
    public class FormulaQuantifier : Formula
    {
        public readonly Quantifier Quantifier;
        public readonly ObjectVariable ObjectVariable;
        public readonly Formula Formula;

        public FormulaQuantifier(Quantifier quantifier, ObjectVariable objectVariable, Formula formula)
        {
            Quantifier = quantifier ?? throw new ArgumentNullException(nameof(quantifier));
            ObjectVariable = objectVariable ?? throw new ArgumentNullException(nameof(objectVariable));
            Formula = formula ?? throw new ArgumentNullException(nameof(formula));
        }

        public override string ToString()
        {
            return $"({Quantifier}{ObjectVariable}){Formula}";
        }

        public override IEnumerable<ObjectVariable> FreeObjectVariables
        {
            get { return Formula.FreeObjectVariables.Where(o => !o.Equals(ObjectVariable)); }
        }
    }
}