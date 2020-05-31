namespace LogicLanguageLib.Alphabet
{
    public sealed class LessPredicate : ArithmeticPredicate
    {
        private static readonly LessPredicate Instance = new LessPredicate("<");

        private LessPredicate(string name) : base(name)
        {
        }

        public static LessPredicate GetInstance()
        {
            return Instance;
        }
    }
}