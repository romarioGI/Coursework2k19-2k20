using System.Collections.Generic;
using LogicLanguageLib.Alphabet;
using SimpleTarskiAlgorithmLib;

namespace SimpleTarskiAlgorithmRunner
{
    public static class Predicates
    {
        public static Predicate Equal = EqualityPredicate.GetInstance();
        public static Predicate More = MorePredicate.GetInstance();
        public static Predicate Less = LessPredicate.GetInstance();
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