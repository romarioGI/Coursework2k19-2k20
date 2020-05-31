using LogicLanguageLib.Alphabet;

namespace SimpleTarskiAlgorithmRunner
{
    public static class Functions
    {
        public static readonly Function Add = Addition.GetInstance();
        public static readonly Function Subtract = Subtraction.GetInstance();
        public static readonly Function UnaryMinus = LogicLanguageLib.Alphabet.UnaryMinus.GetInstance();
        public static readonly Function Multi = Multiplication.GetInstance();
        public static readonly Function Divide = Division.GetInstance();
        public static readonly Function Pow = Exponentiation.GetInstance();
    }
}