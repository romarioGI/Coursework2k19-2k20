using LogicLanguageLib;

namespace SimpleTarskiAlgorithmRunner
{
    public static class Predicates
    {
        public static Predicate Equal = new Predicate("=", 2);
        public static Predicate More = new Predicate(">", 2);
        public static Predicate Less = new Predicate("<", 2);
        public static Predicate EqualZero = new Predicate("=", 1);
        public static Predicate MoreZero = new Predicate(">", 1);
        public static Predicate LessZero = new Predicate("<", 1);
    }
}