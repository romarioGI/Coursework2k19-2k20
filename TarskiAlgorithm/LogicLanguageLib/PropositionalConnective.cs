namespace LogicLanguageLib
{
    public abstract class PropositionalConnective : LogicalSymbol
    {
        public readonly int Arity;

        protected PropositionalConnective(string name, int arity) : base(name)
        {
            Arity = arity;
        }
    }
}