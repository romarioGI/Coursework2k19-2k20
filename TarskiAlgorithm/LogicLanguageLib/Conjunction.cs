namespace LogicLanguageLib
{
    public class Conjunction : BinaryPropositionalConnective
    {
        private static readonly Conjunction Instance = new Conjunction();

        private Conjunction() : base("&")
        {
        }

        public Conjunction GetInstance()
        {
            return Instance;
        }
    }
}