namespace LogicLanguageLib.Alphabet
{
    public abstract class PropositionalConnective : LogicalSymbol
    {
        public readonly int Arity;

        protected PropositionalConnective(int arity)
        {
            Arity = arity;
        }
    }
}