using System;

namespace LogicLanguageLib.Alphabet
{
    public sealed class Conjunction : BinaryPropositionalConnective
    {
        private static readonly Conjunction Instance = new Conjunction();

        private Conjunction()
        {
        }

        public static Conjunction GetInstance()
        {
            return Instance;
        }

        public override string ToString()
        {
            return "&";
        }

        public override int Priority => 40;

        protected override bool EqualsSameType(Symbol other)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("&");
        }
    }
}