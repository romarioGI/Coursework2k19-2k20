using System;

namespace LogicLanguageLib.Alphabet
{
    public sealed class UniversalQuantifier : Quantifier
    {
        private static readonly UniversalQuantifier Instance = new UniversalQuantifier();

        private UniversalQuantifier()
        {
        }

        public static UniversalQuantifier GetInstance()
        {
            return Instance;
        }

        public override string ToString()
        {
            return "∀";
        }

        protected override bool EqualsSameType(Symbol other)
        {
            return other is UniversalQuantifier;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("∀");
        }
    }
}