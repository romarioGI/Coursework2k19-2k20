namespace LogicLanguageLib.Alphabet
{
    public abstract class Quantifier : LogicalSymbol
    {
        public override int Priority => 60;
    }
}