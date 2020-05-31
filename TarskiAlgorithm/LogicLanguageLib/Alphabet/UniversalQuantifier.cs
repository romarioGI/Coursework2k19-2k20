﻿using System;

namespace LogicLanguageLib.Alphabet
{
    public sealed class UniversalQuantifier : Quantifier
    {
        private static readonly UniversalQuantifier Instance = new UniversalQuantifier();

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
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("∀");
        }
    }
}