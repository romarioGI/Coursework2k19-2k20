using LogicLanguageLib;

namespace SimpleTarskiAlgorithmRunner
{
    public static class Functions
    {
        public static readonly Function Add = new Function("+", 2);
        public static readonly Function Subtract = new Function("-", 2);
        public static readonly Function UnaryMinus = new Function("-", 1);
        public static readonly Function Multi = new Function("*", 2);
        public static readonly Function Divide = new Function("/", 2);
        public static readonly Function Pow = new Function("^", 2);
    }
}