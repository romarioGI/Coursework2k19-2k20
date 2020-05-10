namespace LogicLanguageLib
{
    public sealed class Implication : BinaryPropositionalConnective
    {
        private static readonly Implication Instance = new Implication();

        private Implication() : base("→")
        {
        }

        public static Implication GetInstance()
        {
            return Instance;
        }
    }
}