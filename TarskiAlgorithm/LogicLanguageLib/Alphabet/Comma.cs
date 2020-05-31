﻿using System;

namespace LogicLanguageLib.Alphabet
{
    public sealed class Comma : TechnicalSymbol
    {
        private static readonly Comma Instance = new Comma();

        private Comma()
        {
        }

        public static Comma GetInstance()
        {
            return Instance;
        }

        public override string ToString()
        {
            return ",";
        }

        public override int Priority => 10;

        protected override bool EqualsSameType(Symbol other)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(",");
        }
    }
}