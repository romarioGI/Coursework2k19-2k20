using System;

namespace LogicLanguageLib.Alphabet
{
    public sealed class Disjunction : BinaryPropositionalConnective
    {
        private static readonly Disjunction Instance = new Disjunction();

        private Disjunction()
        {
        }

        public static Disjunction GetInstance()
        {
            return Instance;
        }

        public override string ToString()
        {
            return "∨";
        }

        public override int Priority => 30;

        protected override bool EqualsSameType(Symbol other)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("∨");
        }
    }
}