using System;
using System.Collections.Generic;

namespace LogicLanguageLib
{
    public class TermIndividualConstant<T> : Term, IEquatable<TermIndividualConstant<T>>
    {
        public readonly IndividualConstant<T> IndividualConstant;

        public TermIndividualConstant(IndividualConstant<T> individualConstant)
        {
            IndividualConstant = individualConstant;
        }

        public override IEnumerable<ObjectVariable> FreeObjectVariables
        {
            get { yield break; }
        }

        public override bool Equals(Term other)
        {
            return Equals(other as TermIndividualConstant<T>);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IndividualConstant);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TermIndividualConstant<T>) obj);
        }

        public bool Equals(TermIndividualConstant<T> other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return IndividualConstant.Equals(other.IndividualConstant);
        }

        public override string ToString()
        {
            return IndividualConstant.ToString();
        }
    }
}