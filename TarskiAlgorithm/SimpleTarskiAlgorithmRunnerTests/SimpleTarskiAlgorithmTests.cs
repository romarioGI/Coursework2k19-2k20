using LogicLanguageLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTarskiAlgorithmLib;
using SimpleTarskiAlgorithmRunner;

namespace SimpleTarskiAlgorithmRunnerTests
{
    [TestClass]
    public class SimpleTarskiAlgorithmTests
    {
        //(∃x)(x>1/2)
        [TestMethod]
        public void Test1()
        {
            ObjectVariableTerm x = new ObjectVariable("x");

            IndividualConstantTerm<RationalNumber> half =
                (IndividualConstant<RationalNumber>) (new RationalNumber(1, 2));

            var formula1 = new PredicateFormula(Predicates.More, x, half);

            var formula2 = new QuantifierFormula(ExistentialQuantifier.GetInstance(), x, formula1);

            var res = SimpleTarskiAlgorithm.QuantifiersElimination(formula2);

            var expected = new PredicateFormula(True.GetInstance());

            Assert.AreEqual(expected, res);
        }

        //(∃x)((x>1/2)&(x<1))
        [TestMethod]
        public void Test2()
        {
            ObjectVariableTerm x = new ObjectVariable("x");

            IndividualConstantTerm<RationalNumber> half =
                (IndividualConstant<RationalNumber>) (new RationalNumber(1, 2));

            IndividualConstantTerm<RationalNumber> one = (IndividualConstant<RationalNumber>) new RationalNumber(1, 1);

            var pr1 = new PredicateFormula(Predicates.More, x, half);

            var pr2 = new PredicateFormula(Predicates.Less, x, one);

            var f1 = new PropositionalConnectiveFormula(Conjunction.GetInstance(), pr1, pr2);

            var f2 = new QuantifierFormula(ExistentialQuantifier.GetInstance(), x, f1);

            var actual = SimpleTarskiAlgorithm.QuantifiersElimination(f2);

            var expected = new PredicateFormula(True.GetInstance());

            Assert.AreEqual(expected, actual);
        }

        //((∃x)((x>1/2)&(x<1))&(∀x)(>((x^2))∨=(x)))
        [TestMethod]
        public void Test3()
        {
            ObjectVariableTerm x = new ObjectVariable("x");

            IndividualConstantTerm<RationalNumber> half =
                (IndividualConstant<RationalNumber>) (new RationalNumber(1, 2));

            IndividualConstantTerm<RationalNumber>
                one = (IndividualConstant<RationalNumber>) (new RationalNumber(1, 1));

            var pr1 = new PredicateFormula(Predicates.More, x, half);

            var pr2 = new PredicateFormula(Predicates.Less, x, one);

            var f1 = new PropositionalConnectiveFormula(Conjunction.GetInstance(), pr1, pr2);

            var f2 = new QuantifierFormula(ExistentialQuantifier.GetInstance(), x, f1);


            IndividualConstantTerm<int> twoConst = new IndividualConstant<int>(2);
            var xSqr = new FunctionTerm(Functions.Pow, x, twoConst);

            var pr3 = new PredicateFormula(Predicates.MoreZero, xSqr);
            var pr4 = new PredicateFormula(Predicates.EqualZero, x);

            var f3 = new PropositionalConnectiveFormula(Disjunction.GetInstance(), pr3, pr4);
            var f4 = new QuantifierFormula(UniversalQuantifier.GetInstance(), x, f3);

            var f = new PropositionalConnectiveFormula(Conjunction.GetInstance(), f2, f4);
            var actual = SimpleTarskiAlgorithm.QuantifiersElimination(f);

            Formula expected = new PredicateFormula(True.GetInstance());

            Assert.AreEqual(expected, actual);
        }

        //((∃x)((x>1/2)&(x<1))&(>((x^2))∨=(x)))
        [TestMethod]
        public void Test4()
        {
            ObjectVariableTerm x = new ObjectVariable("x");

            IndividualConstantTerm<RationalNumber> half =
                (IndividualConstant<RationalNumber>) (new RationalNumber(1, 2));

            IndividualConstantTerm<RationalNumber>
                one = (IndividualConstant<RationalNumber>) (new RationalNumber(1, 1));

            var pr1 = new PredicateFormula(Predicates.More, x, half);
            var pr2 = new PredicateFormula(Predicates.Less, x, one);

            var f1 = new PropositionalConnectiveFormula(Conjunction.GetInstance(), pr1, pr2);
            var f2 = new QuantifierFormula(ExistentialQuantifier.GetInstance(), x, f1);


            IndividualConstantTerm<int> two = new IndividualConstant<int>(2);
            var xSqr = new FunctionTerm(Functions.Pow, x, two);

            var pr3 = new PredicateFormula(Predicates.MoreZero, xSqr);
            var pr4 = new PredicateFormula(Predicates.EqualZero, x);

            var f3 = new PropositionalConnectiveFormula(Disjunction.GetInstance(), pr3, pr4);

            var f = new PropositionalConnectiveFormula(Conjunction.GetInstance(), f2, f3);
            var actual = SimpleTarskiAlgorithm.QuantifiersElimination(f);

            Formula expected = f3;

            Assert.AreEqual(expected, actual);
        }
    }
}