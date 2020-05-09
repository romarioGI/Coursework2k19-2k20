using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicLanguageLib
{
    public class FormulaQuantifier : Formula, IEquatable<FormulaQuantifier>
    {
        public readonly Quantifier Quantifier;
        public readonly ObjectVariable ObjectVariable;
        public readonly Formula SubFormula;

        public FormulaQuantifier(Quantifier quantifier, ObjectVariable objectVariable, Formula formula)
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

        public bool Equals(FormulaQuantifier other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!Quantifier.Equals(other.Quantifier)) return false;
            if (!ObjectVariable.Equals(other.ObjectVariable)) return false;
            return SubFormula.Equals(other.SubFormula);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FormulaQuantifier) obj);
        }

        public override bool Equals(Formula other)
        {
            return Equals(other as FormulaQuantifier);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Quantifier, ObjectVariable, SubFormula);
        }
    }
}