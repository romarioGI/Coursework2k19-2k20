using System;

namespace LogicLanguageLib
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
            return other is Negation;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("¬");
        }
    }
}