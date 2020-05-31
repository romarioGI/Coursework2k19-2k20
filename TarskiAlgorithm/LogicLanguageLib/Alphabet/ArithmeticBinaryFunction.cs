using System;

namespace LogicLanguageLib.Alphabet
{
    public abstract class ArithmeticBinaryFunction : Function, IEquatable<ArithmeticBinaryFunction>
    {
        protected ArithmeticBinaryFunction(string name) : base(name, 2)
        {
        }

        public bool Equals(ArithmeticBinaryFunction other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode() => base.GetHashCode();

        public static bool IsArithmeticBinaryFunction(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/' || c == '^';
        }

        public static ArithmeticBinaryFunction Factory(char c)
        {
            return c switch
            {
                '+' => (ArithmeticBinaryFunction) Addition.GetInstance(),
                '-' => Subtraction.GetInstance(),
                '*' => Multiplication.GetInstance(),
                '/' => Division.GetInstance(),
                '^' => Exponentiation.GetInstance(),
                _ => throw new ArgumentException($"{c} - is not ArithmeticBinaryFunction")
            };
        }
    }
}