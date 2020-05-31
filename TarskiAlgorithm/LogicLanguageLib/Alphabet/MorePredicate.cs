namespace LogicLanguageLib.Alphabet
{
    public sealed class MorePredicate : ArithmeticPredicate
    {
        private static readonly MorePredicate Instance = new MorePredicate(">");

        private MorePredicate(string name) : base(name)
        {
        }

        public static MorePredicate GetInstance()
        {
            return Instance;
        }
    }
}