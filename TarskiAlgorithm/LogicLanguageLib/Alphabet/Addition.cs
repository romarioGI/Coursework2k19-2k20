namespace LogicLanguageLib.Alphabet
{
    public sealed class Addition : ArithmeticBinaryFunction
    {
        private static readonly Addition Instance = new Addition("+");

        private Addition(string name) : base(name)
        {
        }

        public static Addition GetInstance()
        {
            return Instance;
        }

        public override int Priority => 110;
    }
}