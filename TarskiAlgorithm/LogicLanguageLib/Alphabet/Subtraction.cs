namespace LogicLanguageLib.Alphabet
{
    public sealed class Subtraction : ArithmeticBinaryFunction
    {
        private static readonly Subtraction Instance = new Subtraction("-");

        private Subtraction(string name) : base(name)
        {
        }

        public static Subtraction GetInstance()
        {
            return Instance;
        }

        public override int Priority => 110;
    }
}