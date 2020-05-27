using System;

namespace LogicLanguageLib
{
    public sealed class RightBracket : TechnicalSymbol
    {
        private static readonly RightBracket Instance = new RightBracket();

        public static RightBracket GetInstance()
        {
            return Instance;
        }

        public override string ToString()
        {
            return ")";
        }

        protected override bool EqualsSameType(Symbol other)
        {
            return other is RightBracket;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(")");
        }
    }
}