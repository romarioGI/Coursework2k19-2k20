using System.Collections.Generic;

namespace LogicLanguageLib
{
    public class TermObjectVariable : Term
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
    }
}