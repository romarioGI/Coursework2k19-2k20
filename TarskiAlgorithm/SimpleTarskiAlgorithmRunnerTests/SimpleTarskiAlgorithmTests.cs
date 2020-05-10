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
            var xTerm = new ObjectVariableTerm(x);

            var half = new RationalNumber(1, 2);
            var halfConst = new IndividualConstant<RationalNumber>(half);
            var term = new IndividualConstantTerm<RationalNumber>(halfConst);

            var formula1 = new PredicateFormula(Predicates.More, xTerm, term);

            var formula2 = new QuantifierFormula(
                ExistentialQuantifier.GetInstance(),
                x,
                formula1);

            var res = SimpleTarskiAlgorithm.QuantifiersElimination(formula2);

            var expected = new PredicateFormula(True.GetInstance());

            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void Test2()
        {
            var x = new ObjectVariable("x");
            var xTerm = new ObjectVariableTerm(x);

            var half = new RationalNumber(1, 2);
            var halfConst = new IndividualConstant<RationalNumber>(half);
            var halfTerm = new IndividualConstantTerm<RationalNumber>(halfConst);

            var one = new RationalNumber(1, 1);
            var oneConst = new IndividualConstant<RationalNumber>(one);
            var oneTerm = new IndividualConstantTerm<RationalNumber>(oneConst);

            var pr1 = new PredicateFormula(Predicates.More, xTerm, halfTerm);
            var pr2 = new PredicateFormula(Predicates.Less, xTerm, oneTerm);

            var f1 = new PropositionalConnectiveFormula(Conjunction.GetInstance(), pr1, pr2);
            var f2 = new QuantifierFormula(ExistentialQuantifier.GetInstance(), x, f1);

            var actual = SimpleTarskiAlgorithm.QuantifiersElimination(f2);

            var expected = new PredicateFormula(True.GetInstance());

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test3()
        {
            var x = new ObjectVariable("x");
            var xTerm = new ObjectVariableTerm(x);

            var half = new RationalNumber(1, 2);
            var halfConst = new IndividualConstant<RationalNumber>(half);
            var halfTerm = new IndividualConstantTerm<RationalNumber>(halfConst);

            var one = new RationalNumber(1, 1);
            var oneConst = new IndividualConstant<RationalNumber>(one);
            var oneTerm = new IndividualConstantTerm<RationalNumber>(oneConst);

            var pr1 = new PredicateFormula(Predicates.More, xTerm, halfTerm);
            var pr2 = new PredicateFormula(Predicates.Less, xTerm, oneTerm);

            var f1 = new PropositionalConnectiveFormula(Conjunction.GetInstance(), pr1, pr2);
            var f2 = new QuantifierFormula(ExistentialQuantifier.GetInstance(), x, f1);


            var twoConst = new IndividualConstant<int>(2);
            var twoTerm = new IndividualConstantTerm<int>(twoConst);
            var xxTerm = new FunctionTerm(Functions.Pow, xTerm, twoTerm);

            var pr3 = new PredicateFormula(Predicates.MoreZero, xxTerm);
            var pr4 = new PredicateFormula(Predicates.EqualZero, xTerm);

            var f3 = new PropositionalConnectiveFormula(Disjunction.GetInstance(), pr3, pr4);
            var f4 = new QuantifierFormula(UniversalQuantifier.GetInstance(), x, f3);

            var f = new PropositionalConnectiveFormula(Conjunction.GetInstance(), f2, f4);
            var actual = SimpleTarskiAlgorithm.QuantifiersElimination(f);

            Formula expected = new PredicateFormula(True.GetInstance());
            expected = new PropositionalConnectiveFormula(Conjunction.GetInstance(), expected, expected);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test4()
        {
            var x = new ObjectVariable("x");
            var xTerm = new ObjectVariableTerm(x);

            var half = new RationalNumber(1, 2);
            var halfConst = new IndividualConstant<RationalNumber>(half);
            var halfTerm = new IndividualConstantTerm<RationalNumber>(halfConst);

            var one = new RationalNumber(1, 1);
            var oneConst = new IndividualConstant<RationalNumber>(one);
            var oneTerm = new IndividualConstantTerm<RationalNumber>(oneConst);

            var pr1 = new PredicateFormula(Predicates.More, xTerm, halfTerm);
            var pr2 = new PredicateFormula(Predicates.Less, xTerm, oneTerm);

            var f1 = new PropositionalConnectiveFormula(Conjunction.GetInstance(), pr1, pr2);
            var f2 = new QuantifierFormula(ExistentialQuantifier.GetInstance(), x, f1);


            var twoConst = new IndividualConstant<int>(2);
            var twoTerm = new IndividualConstantTerm<int>(twoConst);
            var xxTerm = new FunctionTerm(Functions.Pow, xTerm, twoTerm);

            var pr3 = new PredicateFormula(Predicates.MoreZero, xxTerm);
            var pr4 = new PredicateFormula(Predicates.EqualZero, xTerm);

            var f3 = new PropositionalConnectiveFormula(Disjunction.GetInstance(), pr3, pr4);

            var f = new PropositionalConnectiveFormula(Conjunction.GetInstance(), f2, f3);
            var actual = SimpleTarskiAlgorithm.QuantifiersElimination(f);

            Formula expected = new PredicateFormula(True.GetInstance());
            expected = new PropositionalConnectiveFormula(Conjunction.GetInstance(), expected, f3);

            Assert.AreEqual(expected, actual);
        }
    }
}