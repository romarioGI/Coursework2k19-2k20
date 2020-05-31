using System;

namespace LogicLanguageLib.Alphabet
{
    public abstract class ArithmeticPredicate : Predicate, IEquatable<ArithmeticPredicate>
    {
        protected ArithmeticPredicate(string name) : base(name, 2)
        {
        }

        public bool Equals(ArithmeticPredicate other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode() => base.GetHashCode();

        public static bool IsArithmeticPredicate(char c)
        {
            return c == '=' || c == '<' || c == '>';
        }

        public static ArithmeticPredicate Factory(char c)
        {
            return c switch
            {
                '<' => LessPredicate.GetInstance(),
                '=' => EqualityPredicate.GetInstance(),
                '>' => MorePredicate.GetInstance(),
                _ => throw new ArgumentException($"{c} - is not a predicate")
            };
        }
    }
}