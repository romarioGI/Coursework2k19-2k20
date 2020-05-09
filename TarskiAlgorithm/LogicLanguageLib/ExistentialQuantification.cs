namespace LogicLanguageLib
{
    public class ExistentialQuantification : Quantifier
    {
        private static readonly ExistentialQuantification Instance = new ExistentialQuantification();

        private ExistentialQuantification() : base("∃")
        {
        }

        public ExistentialQuantification GetInstance()
        {
            return Instance;
        }
    }
}