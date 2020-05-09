﻿namespace LogicLanguageLib
{
    public class Negation : UnaryPropositionalConnective
    {
        private static readonly Negation Instance = new Negation();

        private Negation() : base("¬")
        {
        }

        public Negation GetInstance()
        {
            return Instance;
        }
    }
}