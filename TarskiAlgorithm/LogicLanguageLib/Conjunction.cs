﻿namespace LogicLanguageLib
{
    public sealed class Conjunction : BinaryPropositionalConnective
    {
        private static readonly Conjunction Instance = new Conjunction();

        private Conjunction() : base("&")
        {
        }

        public static Conjunction GetInstance()
        {
            return Instance;
        }
    }
}