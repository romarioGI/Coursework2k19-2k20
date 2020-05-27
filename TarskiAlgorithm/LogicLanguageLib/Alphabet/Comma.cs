using System;

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

        protected override bool EqualsSameType(Symbol other)
        {
            return other is Comma;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(",");
        }
    }
}