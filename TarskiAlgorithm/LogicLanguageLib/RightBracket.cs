namespace LogicLanguageLib
{
    public sealed class RightBracket : TechnicalSymbol
    {
        private static readonly RightBracket Instance = new RightBracket();

        private RightBracket() : base(")")
        {
        }

        public static RightBracket GetInstance()
        {
            return Instance;
        }
    }
}