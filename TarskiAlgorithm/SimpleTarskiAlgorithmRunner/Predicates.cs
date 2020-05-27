using System.Collections.Generic;
using LogicLanguageLib.Alphabet;
using SimpleTarskiAlgorithmLib;

namespace SimpleTarskiAlgorithmRunner
{
    public static class Predicates
    {
        public static Predicate Equal = new ArithmeticPredicate("=");
        public static Predicate More = new ArithmeticPredicate(">");
        public static Predicate Less = new ArithmeticPredicate("<");
        public static Predicate EqualZero = new Predicate("=", 1);
        public static Predicate MoreZero = new Predicate(">", 1);
        public static Predicate LessZero = new Predicate("<", 1);

        public static Dictionary<Predicate, Sign> Interpretations = new Dictionary<Predicate, Sign>
        {
            {Equal, Sign.Zero},
            {More, Sign.MoreZero},
            {Less, Sign.LessZero},
            {EqualZero, Sign.Zero},
            {MoreZero, Sign.MoreZero},
            {LessZero, Sign.LessZero},
        };
    }
}