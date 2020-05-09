namespace LogicLanguageLib
{
    public class False : Predicate
    {
        private static readonly False Instance = new False();

        public static False GetInstance()
        {
            return Instance;
        }

        private False() : base("FALSE", 0)
        {
        }
    }
}