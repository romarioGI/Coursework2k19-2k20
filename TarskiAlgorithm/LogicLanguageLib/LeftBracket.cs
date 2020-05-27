using System;

namespace LogicLanguageLib
{
    public sealed class LeftBracket : TechnicalSymbol
    {
        private static readonly LeftBracket Instance = new LeftBracket();

        public static LeftBracket GetInstance()
        {
            return Instance;
        }

        public override string ToString()
        {
            return "(";
        }

        protected override bool EqualsSameType(Symbol other)
        {
            return other is LeftBracket;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("(");
        }
    }
}