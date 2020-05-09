using System;
using System.Collections.Generic;
using System.Text;
using LogicLanguageLib;
using SimpleTarskiAlgorithmLib;

namespace SimpleTarskiAlgorithmRunner
{
    public static class FormulaParser
    {
        public static Formula ToFormula(IEnumerable<Symbol> symbols)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<Symbol> ToSymbols(string str)
        {
            throw new NotImplementedException();
        }

        public static Formula ToFormula(string str)
        {
            return ToFormula(ToSymbols(str));
        }

        public static Formula ToFormula(IEnumerable<Formula> formulas, PropositionalConnective propositionalConnective)
        {
            throw new NotImplementedException();
        }

        public static (Polynomial, Sign) ToPolynomialAndSign(FormulaPredicate formulaPredicate)
        {
            throw new NotImplementedException();
        }
    }
}