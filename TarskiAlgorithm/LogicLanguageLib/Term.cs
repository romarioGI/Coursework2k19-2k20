using System.Collections.Generic;

namespace LogicLanguageLib
{
    public abstract class Term
    {
        public abstract IEnumerable<ObjectVariable> FreeObjectVariables { get; }
    }
}