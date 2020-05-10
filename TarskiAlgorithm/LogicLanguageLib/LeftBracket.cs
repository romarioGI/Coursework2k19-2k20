namespace LogicLanguageLib
{
    public sealed class LeftBracket : TechnicalSymbol
    {
        private static readonly LeftBracket Instance = new LeftBracket();

        private LeftBracket() : base("(")
        {
        }

        public static LeftBracket GetInstance()
        {
            return Instance;
        }
    }
}