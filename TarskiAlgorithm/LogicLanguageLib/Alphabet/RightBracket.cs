﻿using System;

namespace LogicLanguageLib.Alphabet
{
    public sealed class RightBracket : TechnicalSymbol
    {
        private static readonly RightBracket Instance = new RightBracket();

        public static RightBracket GetInstance()
        {
            return Instance;
        }

        public override string ToString()
        {
            return ")";
        }

        public override int Priority => 10;

        protected override bool EqualsSameType(Symbol other)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(")");
        }
    }
}