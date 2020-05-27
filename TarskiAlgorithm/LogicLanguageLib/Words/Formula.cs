using System;
using System.Collections.Generic;
using System.Linq;
using LogicLanguageLib.Alphabet;

namespace LogicLanguageLib.Words
{
    public abstract class Formula : IEquatable<Formula>
    {
        public abstract IEnumerable<ObjectVariable> FreeObjectVariables { get; }

        public bool IsSentence => !FreeObjectVariables.Any();

        public abstract bool Equals(Formula other);

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Formula) obj);
        }

        public abstract override int GetHashCode();

        public abstract override string ToString();
    }
}