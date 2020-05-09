using LogicLanguageLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTarskiAlgorithmLib;
using SimpleTarskiAlgorithmRunner;

namespace SimpleTarskiAlgorithmRunnerTests
{
    [TestClass]
    public class SimpleTarskiAlgorithmTests
    {
        [TestMethod]
        public void Test1()
        {
            var x = new ObjectVariable("x");
            var xTerm = new TermObjectVariable(x);

            var half = new RationalNumber(1, 2);
            var halfConst = new IndividualConstant<RationalNumber>(half);
            var term = new TermIndividualConstant<RationalNumber>(halfConst);

            var formula1 = new FormulaPredicate(Predicates.More, xTerm, term);

            var formula2 = new FormulaQuantifier(
                ExistentialQuantifier.GetInstance(),
                x,
                formula1);

            var res = SimpleTarskiAlgorithm.QuantifiersElimination(formula2);

            var expected = new FormulaPredicate(True.GetInstance());

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void Test2()
        {
            var x = new ObjectVariable("x");
            var xTerm = new TermObjectVariable(x);

            var half = new RationalNumber(1, 2);
            var halfConst = new IndividualConstant<RationalNumber>(half);
            var halfTerm = new TermIndividualConstant<RationalNumber>(halfConst);

            var one = new RationalNumber(1, 1);
            var oneConst = new IndividualConstant<RationalNumber>(one);
            var oneTerm = new TermIndividualConstant<RationalNumber>(oneConst);

            var pr1 = new FormulaPredicate(Predicates.More, xTerm, halfTerm);
            var pr2 = new FormulaPredicate(Predicates.Less, xTerm, oneTerm);

            var f1 = new FormulaPropositionalConnective(Conjunction.GetInstance(), pr1, pr2);
            var f2 = new FormulaQuantifier(ExistentialQuantifier.GetInstance(), x, f1);

            var actual = SimpleTarskiAlgorithm.QuantifiersElimination(f2);

            var expected = new FormulaPredicate(True.GetInstance());

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test3()
        {
            var x = new ObjectVariable("x");
            var xTerm = new TermObjectVariable(x);

            var half = new RationalNumber(1, 2);
            var halfConst = new IndividualConstant<RationalNumber>(half);
            var halfTerm = new TermIndividualConstant<RationalNumber>(halfConst);

            var one = new RationalNumber(1, 1);
            var oneConst = new IndividualConstant<RationalNumber>(one);
            var oneTerm = new TermIndividualConstant<RationalNumber>(oneConst);

            var pr1 = new FormulaPredicate(Predicates.More, xTerm, halfTerm);
            var pr2 = new FormulaPredicate(Predicates.Less, xTerm, oneTerm);

            var f1 = new FormulaPropositionalConnective(Conjunction.GetInstance(), pr1, pr2);
            var f2 = new FormulaQuantifier(ExistentialQuantifier.GetInstance(), x, f1);


            var twoConst = new IndividualConstant<int>(2);
            var twoTerm = new TermIndividualConstant<int>(twoConst);
            var xxTerm = new TermFunction(Functions.Pow, xTerm, twoTerm);

            var pr3 = new FormulaPredicate(Predicates.MoreZero, xxTerm);
            var pr4 = new FormulaPredicate(Predicates.EqualZero, xTerm);

            var f3 = new FormulaPropositionalConnective(Disjunction.GetInstance(), pr3, pr4);
            var f4 = new FormulaQuantifier(UniversalQuantifier.GetInstance(), x, f3);

            var f = new FormulaPropositionalConnective(Conjunction.GetInstance(), f2, f4);
            var actual = SimpleTarskiAlgorithm.QuantifiersElimination(f);

            Formula expected = new FormulaPredicate(True.GetInstance());
            expected = new FormulaPropositionalConnective(Conjunction.GetInstance(), expected, expected);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test4()
        {
            var x = new ObjectVariable("x");
            var xTerm = new TermObjectVariable(x);

            var half = new RationalNumber(1, 2);
            var halfConst = new IndividualConstant<RationalNumber>(half);
            var halfTerm = new TermIndividualConstant<RationalNumber>(halfConst);

            var one = new RationalNumber(1, 1);
            var oneConst = new IndividualConstant<RationalNumber>(one);
            var oneTerm = new TermIndividualConstant<RationalNumber>(oneConst);

            var pr1 = new FormulaPredicate(Predicates.More, xTerm, halfTerm);
            var pr2 = new FormulaPredicate(Predicates.Less, xTerm, oneTerm);

            var f1 = new FormulaPropositionalConnective(Conjunction.GetInstance(), pr1, pr2);
            var f2 = new FormulaQuantifier(ExistentialQuantifier.GetInstance(), x, f1);


            var twoConst = new IndividualConstant<int>(2);
            var twoTerm = new TermIndividualConstant<int>(twoConst);
            var xxTerm = new TermFunction(Functions.Pow, xTerm, twoTerm);

            var pr3 = new FormulaPredicate(Predicates.MoreZero, xxTerm);
            var pr4 = new FormulaPredicate(Predicates.EqualZero, xTerm);

            var f3 = new FormulaPropositionalConnective(Disjunction.GetInstance(), pr3, pr4);

            var f = new FormulaPropositionalConnective(Conjunction.GetInstance(), f2, f3);
            var actual = SimpleTarskiAlgorithm.QuantifiersElimination(f);

            Formula expected = new FormulaPredicate(True.GetInstance());
            expected = new FormulaPropositionalConnective(Conjunction.GetInstance(), expected, f3);

            Assert.AreEqual(expected, actual);
        }
    }
}