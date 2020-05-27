using System;
using System.Collections.Generic;
using System.Linq;
using LogicLanguageLib.Alphabet;

namespace LogicLanguageLib.Words
{
    public class QuantifierFormula : Formula, IEquatable<QuantifierFormula>
    {
        public readonly Quantifier Quantifier;
        public readonly ObjectVariable ObjectVariable;
        public readonly Formula SubFormula;

        public QuantifierFormula(Quantifier quantifier, ObjectVariable objectVariable, Formula formula)
        {
            Quantifier = quantifier ?? throw new ArgumentNullException(nameof(quantifier));
            ObjectVariable = objectVariable ?? throw new ArgumentNullException(nameof(objectVariable));
            SubFormula = formula ?? throw new ArgumentNullException(nameof(formula));
        }

        public override string ToString()
        {
            return $"({Quantifier}{ObjectVariable}){SubFormula}";
        }

        public override IEnumerable<ObjectVariable> FreeObjectVariables
        {
            get { return SubFormula.FreeObjectVariables.Where(o => !o.Equals(ObjectVariable)); }
        }

        public bool Equals(QuantifierFormula other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Quantifier.Equals(other.Quantifier) &&
                   ObjectVariable.Equals(other.ObjectVariable) &&
                   SubFormula.Equals(other.SubFormula);
        }

        public override bool Equals(Formula other)
        {
            return Equals(other as QuantifierFormula);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Quantifier, ObjectVariable, SubFormula);
        }
    }
}