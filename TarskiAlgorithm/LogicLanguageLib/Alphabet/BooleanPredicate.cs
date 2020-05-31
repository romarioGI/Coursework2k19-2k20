namespace LogicLanguageLib.Alphabet
{
    public abstract class BooleanPredicate : Predicate
    {
        protected BooleanPredicate(string name) : base(name, 0)
        {
        }

        public override int Priority => 90;
    }
}