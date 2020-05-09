using System.Collections.Generic;
using System.Linq;

namespace LogicLanguageLib
{
    public abstract class Formula
    {
        public abstract IEnumerable<ObjectVariable> FreeObjectVariables { get; }

        public bool IsSentence => !FreeObjectVariables.Any();
    }
}