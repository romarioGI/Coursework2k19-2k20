namespace LogicLanguageLib
{
    public class Disjunction : BinaryPropositionalConnective
    {
        private static readonly Disjunction Instance = new Disjunction();

        private Disjunction() : base("∨")
        {
        }

        public static Disjunction GetInstance()
        {
            return Instance;
        }
    }
}