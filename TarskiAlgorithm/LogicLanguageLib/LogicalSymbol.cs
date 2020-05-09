using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLanguageLib
{
    public abstract class LogicalSymbol: Symbol
    {
        protected LogicalSymbol(string name) : base(name)
        {
        }
    }
}
