using System;
using System.Collections.Generic;

namespace LogicLanguageLib
{
    public class ObjectVariableTerm : Term, IEquatable<ObjectVariableTerm>
    {
        public readonly ObjectVariable ObjectVariable;

        public ObjectVariableTerm(ObjectVariable objectVariable)
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

        public bool Equals(ObjectVariableTerm other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return ObjectVariable.Equals(other.ObjectVariable);
        }

        public override bool Equals(Term other)
        {
            return Equals(other as ObjectVariableTerm);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ObjectVariable);
        }

        public static implicit operator ObjectVariable(ObjectVariableTerm term)
        {
            return term.ObjectVariable;
        }

        public static implicit operator ObjectVariableTerm(ObjectVariable objectVariable)
        {
            return new ObjectVariableTerm(objectVariable);
        }
    }
}