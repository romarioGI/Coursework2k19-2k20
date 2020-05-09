namespace LogicLanguageLib
{
    public class Implication : BinaryPropositionalConnective
    {
        private static readonly Implication Instance = new Implication();

        private Implication() : base("→")
        {
        }

        public Implication GetInstance()
        {
            return Instance;
        }
    }
}