using System;

namespace LogicLanguageLib.Alphabet
{
    public sealed class Negation : UnaryPropositionalConnective
    {
        private static readonly Negation Instance = new Negation();

        private Negation()
        {
        }

        public static Negation GetInstance()
        {
            return Instance;
        }

        public override string ToString()
        {
            return "¬";
        }

        protected override bool EqualsSameType(Symbol other)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("¬");
        }
    }
}