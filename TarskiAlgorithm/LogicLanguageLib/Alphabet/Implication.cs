using System;

namespace LogicLanguageLib.Alphabet
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

        public override int Priority => 20;

        protected override bool EqualsSameType(Symbol other)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("→");
        }
    }
}