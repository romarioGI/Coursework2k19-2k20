using LogicLanguageLib.Alphabet;

namespace SimpleTarskiAlgorithmRunner
{
    public static class Functions
    {
        public static readonly Function Add = new ArithmeticFunction("+");
        public static readonly Function Subtract = new ArithmeticFunction("-");
        public static readonly Function UnaryMinus = new Function("-", 1);
        public static readonly Function Multi = new ArithmeticFunction("*");
        public static readonly Function Divide = new ArithmeticFunction("/");
        public static readonly Function Pow = new ArithmeticFunction("^");
    }
}