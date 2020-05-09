namespace LogicLanguageLib
{
    public class UniversalQuantifier : Quantifier
    {
        private static readonly UniversalQuantifier Instance = new UniversalQuantifier();

        private UniversalQuantifier() : base("∀")
        {
        }

        public static UniversalQuantifier GetInstance()
        {
            return Instance;
        }
    }
}