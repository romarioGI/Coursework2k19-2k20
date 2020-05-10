﻿namespace LogicLanguageLib
{
    public sealed class Negation : UnaryPropositionalConnective
    {
        private static readonly Negation Instance = new Negation();

        private Negation() : base("¬")
        {
        }

        public static Negation GetInstance()
        {
            return Instance;
        }
    }
}