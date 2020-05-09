namespace LogicLanguageLib
{
    public class LeftBracket : TechnicalSymbol
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