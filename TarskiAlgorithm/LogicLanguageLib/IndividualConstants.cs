namespace LogicLanguageLib
{
    public class IndividualConstants<T> : NonLogicalSymbol
    {
        public readonly T Value;

        public IndividualConstants(string name, T value) : base(name)
        {
            Value = value;
        }
    }
}