namespace LogicLanguageLib
{
    public class RightBracket : TechnicalSymbol
    {
        private static readonly RightBracket Instance = new RightBracket();

        private RightBracket() : base(")")
        {
        }

        public RightBracket GetInstance()
        {
            return Instance;
        }
    }
}