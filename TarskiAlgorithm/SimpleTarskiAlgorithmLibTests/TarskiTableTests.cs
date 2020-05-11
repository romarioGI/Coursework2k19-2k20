using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTarskiAlgorithmLib;

namespace SimpleTarskiAlgorithmLibTests
{
    [TestClass]
    public class TarskiTableTests
    {
        private static readonly VariableName XName = new VariableName("x");

        [TestMethod]
        public void Test1()
        {
            var polynomials = new List<Polynomial> {new Polynomial(new List<RationalNumber> {5}, XName)};
            var saturatedSystem = SimpleSaturator.Saturate(polynomials).ToList();
            var table = new SimpleTarskiTable(saturatedSystem);
            var signs = table[polynomials[0]].ToList();

            var expected = new List<Sign> {Sign.MoreZero, Sign.MoreZero};

            CollectionAssert.AreEqual(expected, signs);
        }

        [TestMethod]
        public void Test2()
        {
            var polynomials = new List<Polynomial> {new Polynomial(new List<RationalNumber> {1, 1, 0, 1}, XName)};
            var saturatedSystem = SimpleSaturator.Saturate(polynomials).ToList();
            var table = new SimpleTarskiTable(saturatedSystem);
            var signs = table[polynomials[0]].ToList();

            var expected = new List<Sign> {Sign.LessZero, Sign.LessZero, Sign.Zero, Sign.MoreZero, Sign.MoreZero};

            CollectionAssert.AreEquivalent(expected, signs);
        }

        [TestMethod]
        public void Test3()
        {
            var polynomials = new List<Polynomial> {new Polynomial(new List<RationalNumber> {0, 0, 1}, XName)};
            var saturatedSystem = SimpleSaturator.Saturate(polynomials).ToList();
            var table = new SimpleTarskiTable(saturatedSystem);
            var signs = table[polynomials[0]].ToList();

            var expected = new List<Sign> {Sign.MoreZero, Sign.Zero, Sign.MoreZero};

            CollectionAssert.AreEquivalent(expected, signs);
        }

        [TestMethod]
        public void Test4()
        {
            var polynomials = new List<Polynomial> {new Polynomial(new List<RationalNumber> {1, -1, -1, 1}, XName)};
            var saturatedSystem = SimpleSaturator.Saturate(polynomials).ToList();
            var table = new SimpleTarskiTable(saturatedSystem);
            var signs = table[polynomials[0]].ToList();

            var expected = new List<Sign>
                {Sign.LessZero, Sign.Zero, Sign.MoreZero, Sign.MoreZero, Sign.Zero, Sign.MoreZero};

            CollectionAssert.AreEquivalent(expected, signs);
        }

        [TestMethod]
        public void Test5()
        {
            var polynomials = new List<Polynomial> {new Polynomial(new List<RationalNumber> {0}, XName)};
            var saturatedSystem = SimpleSaturator.Saturate(polynomials).ToList();
            var table = new SimpleTarskiTable(saturatedSystem);
            var signs = table[polynomials[0]].ToList();

            var expected = new List<Sign> {Sign.Zero, Sign.Zero};

            CollectionAssert.AreEquivalent(expected, signs);
        }
    }
}