namespace LogicLanguageLib
{
    public sealed class True : BooleanPredicate
    {
        private static readonly True Instance = new True();

        public static True GetInstance()
        {
            return Instance;
        }

        private True() : base("TRUE")
        {
        }
    }
}