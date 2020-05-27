using System;
using System.Collections.Generic;
using LogicLanguageLib.Alphabet;

namespace LogicLanguageLib.Words
{
    public abstract class Term : IEquatable<Term>
    {
        public abstract IEnumerable<ObjectVariable> FreeObjectVariables { get; }

        public abstract bool Equals(Term other);

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Term) obj);
        }

        public abstract override int GetHashCode();

        public abstract override string ToString();
    }
}