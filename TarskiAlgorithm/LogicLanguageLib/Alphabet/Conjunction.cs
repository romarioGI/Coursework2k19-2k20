﻿using System;

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

        protected override bool EqualsSameType(Symbol other)
        {
            return other is Conjunction;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine("&");
        }
    }
}