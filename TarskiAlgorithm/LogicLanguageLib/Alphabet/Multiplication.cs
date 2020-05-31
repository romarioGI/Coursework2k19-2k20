namespace LogicLanguageLib.Alphabet
{
    public sealed class Multiplication : ArithmeticBinaryFunction
    {
        private static readonly Multiplication Instance = new Multiplication("*");

        private Multiplication(string name) : base(name)
        {
        }

        public static Multiplication GetInstance()
        {
            return Instance;
        }
    }
}