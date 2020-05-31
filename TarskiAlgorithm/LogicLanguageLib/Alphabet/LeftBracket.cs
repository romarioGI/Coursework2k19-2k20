using System;

namespace LogicLanguageLib.Alphabet
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

        public override int Priority => 0;

        protected override bool EqualsSameType(Symbol other)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("(");
        }
    }
}