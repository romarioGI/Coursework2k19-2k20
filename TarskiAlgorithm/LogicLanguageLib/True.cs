namespace LogicLanguageLib
{
    public class True : Predicate
    {
        private static readonly True Instance = new True();

        public static True GetInstance()
        {
            return Instance;
        }

        private True() : base("TRUE", 0)
        {
        }
    }
}