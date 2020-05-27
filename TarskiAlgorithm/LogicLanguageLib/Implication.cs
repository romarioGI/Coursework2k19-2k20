using System;

namespace LogicLanguageLib
{
    public sealed class Implication : BinaryPropositionalConnective
    {
        private static readonly Implication Instance = new Implication();

        private Implication()
        {
        }

        public static Implication GetInstance()
        {
            return Instance;
        }

        public override string ToString()
        {
            return "→";
        }

        protected override bool EqualsSameType(Symbol other)
        {
            return other is Implication;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("→");
        }
    }
}