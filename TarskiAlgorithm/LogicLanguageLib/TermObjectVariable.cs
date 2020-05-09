using System;
using System.Collections.Generic;

namespace LogicLanguageLib
{
    public class TermObjectVariable : Term, IEquatable<TermObjectVariable>
    {
        public readonly ObjectVariable ObjectVariable;

        public TermObjectVariable(ObjectVariable objectVariable)
        {
            ObjectVariable = objectVariable;
        }

        public override string ToString()
        {
            return ObjectVariable.ToString();
        }

        public override IEnumerable<ObjectVariable> FreeObjectVariables
        {
            get { yield return ObjectVariable; }
        }

        public bool Equals(TermObjectVariable other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return ObjectVariable.Equals(other.ObjectVariable);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TermObjectVariable) obj);
        }

        public override bool Equals(Term other)
        {
            return Equals(other as TermObjectVariable);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ObjectVariable);
        }
    }
}