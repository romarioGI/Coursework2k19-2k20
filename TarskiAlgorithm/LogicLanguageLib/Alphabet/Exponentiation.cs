namespace LogicLanguageLib.Alphabet
{
    public sealed class Exponentiation : ArithmeticBinaryFunction
    {
        private static readonly Exponentiation Instance = new Exponentiation("^");

        private Exponentiation(string name) : base(name)
        {
        }

        public static Exponentiation GetInstance()
        {
            return Instance;
        }
    }
}