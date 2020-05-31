namespace LogicLanguageLib.Alphabet
{
    public sealed class EqualityPredicate : ArithmeticPredicate
    {
        private static readonly EqualityPredicate Instance = new EqualityPredicate("=");

        private EqualityPredicate(string name) : base(name)
        {
        }

        public static EqualityPredicate GetInstance()
        {
            return Instance;
        }
    }
}