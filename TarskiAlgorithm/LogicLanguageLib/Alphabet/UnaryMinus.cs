namespace LogicLanguageLib.Alphabet
{
    public sealed class UnaryMinus : Function
    {
        private static readonly UnaryMinus Instance = new UnaryMinus();

        private UnaryMinus() : base("-", 1)
        {
        }

        public static UnaryMinus GetInstance()
        {
            return Instance;
        }
    }
}