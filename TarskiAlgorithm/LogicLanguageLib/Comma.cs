namespace LogicLanguageLib
{
    public sealed class Comma : TechnicalSymbol
    {
        private static readonly Comma Instance = new Comma();

        private Comma() : base(",")
        {
        }

        public static Comma GetInstance()
        {
            return Instance;
        }
    }
}