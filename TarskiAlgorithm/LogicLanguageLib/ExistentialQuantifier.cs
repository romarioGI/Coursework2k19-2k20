using System;

namespace LogicLanguageLib
{
    public sealed class ExistentialQuantifier : Quantifier
    {
        private static readonly ExistentialQuantifier Instance = new ExistentialQuantifier();

        private ExistentialQuantifier()
        {
        }

        public static ExistentialQuantifier GetInstance()
        {
            return Instance;
        }

        public override string ToString()
        {
            return "∃";
        }

        protected override bool EqualsSameType(Symbol other)
        {
            return other is ExistentialQuantifier;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("∃");
        }
    }
}