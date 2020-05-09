namespace LogicLanguageLib
{
    public abstract class UnaryPropositionalConnective : PropositionalConnective
    {
        protected UnaryPropositionalConnective(string name) : base(name, 1)
        {
        }
    }
}