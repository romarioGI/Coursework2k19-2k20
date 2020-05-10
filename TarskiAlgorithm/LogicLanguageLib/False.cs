namespace LogicLanguageLib
{
    public sealed class False : BooleanPredicate
    {
        private static readonly False Instance = new False();

        public static False GetInstance()
        {
            return Instance;
        }

        private False() : base("FALSE")
        {
        }
    }
}