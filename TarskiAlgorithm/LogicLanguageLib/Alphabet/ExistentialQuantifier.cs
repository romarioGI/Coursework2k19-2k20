using System;

namespace LogicLanguageLib.Alphabet
{
    public sealed class ExistentialQuantifier : Quantifier
    {
        private static readonly ExistentialQuantifier Instance = new ExistentialQuantifier();

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
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("∃");
        }
    }
}