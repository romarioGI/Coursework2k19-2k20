﻿namespace LogicLanguageLib
{
    public sealed class ExistentialQuantifier : Quantifier
    {
        private static readonly ExistentialQuantifier Instance = new ExistentialQuantifier();

        private ExistentialQuantifier() : base("∃")
        {
        }

        public static ExistentialQuantifier GetInstance()
        {
            return Instance;
        }
    }
}