using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTarskiAlgorithmLib;

namespace SimpleTarskiAlgorithmLibTests
{
    [TestClass]
    public class SaturatorTests
    {
        private static VariableDomain _xDomain = new VariableDomain("x");

        [TestMethod]
        public void Test1()
        {
            try
            {
                var polynomials = new List<Polynomial> {new Polynomial(new List<RationalNumber>() {0}, _xDomain)};
                var _ = Saturator.Saturate(polynomials).ToList();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentException)
            {
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
        }

        [TestMethod]
        public void Test2()
        {
            var polynomials = new List<Polynomial> {new Polynomial(new List<RationalNumber> {5}, _xDomain)};
            var saturatedSystem = Saturator.Saturate(polynomials).ToList();

            var expected = new List<Polynomial> {new Polynomial(new List<RationalNumber> {5}, _xDomain)};

            CollectionAssert.AreEqual(expected, saturatedSystem);
        }

        [TestMethod]
        public void Test3()
        {
            var polynomials = new List<Polynomial> {new Polynomial(new List<RationalNumber> {1, 1, 0, 1}, _xDomain)};
            var saturatedSystem = Saturator.Saturate(polynomials).ToList();

            var expected = new List<Polynomial>
            {
                new Polynomial(new List<RationalNumber> {-9}, _xDomain),
                new Polynomial(new List<RationalNumber> {new RationalNumber(-31, 8)}, _xDomain),
                new Polynomial(new List<RationalNumber> {new RationalNumber(2, 3)}, _xDomain),
                new Polynomial(new List<RationalNumber> {1}, _xDomain),
                new Polynomial(new List<RationalNumber> {6}, _xDomain),
                new Polynomial(new List<RationalNumber> {new RationalNumber(31, 4)}, _xDomain),
                new Polynomial(new List<RationalNumber> {1, new RationalNumber(2, 3)}, _xDomain),
                new Polynomial(new List<RationalNumber> {0, 6}, _xDomain),
                new Polynomial(new List<RationalNumber> {1, 0, 3}, _xDomain),
                new Polynomial(new List<RationalNumber> {1, 1, 0, 1}, _xDomain)
            };

            CollectionAssert.AreEquivalent(expected, saturatedSystem);
        }

        [TestMethod]
        public void Test4()
        {
            var polynomials = new List<Polynomial> {new Polynomial(new List<RationalNumber> {0, 0, 1}, _xDomain)};
            var saturatedSystem = Saturator.Saturate(polynomials).ToList();

            var expected = new List<Polynomial>
            {
                new Polynomial(new List<RationalNumber> {2}, _xDomain),
                new Polynomial(new List<RationalNumber> {0, 2}, _xDomain),
                new Polynomial(new List<RationalNumber> {0, 0, 1}, _xDomain),
            };

            CollectionAssert.AreEquivalent(expected, saturatedSystem);
        }

        [TestMethod]
        public void Test5()
        {
            var polynomials = new List<Polynomial> {new Polynomial(new List<RationalNumber> {1, -1, -1, 1}, _xDomain)};
            var saturatedSystem = Saturator.Saturate(polynomials).ToList();

            var expected = new List<Polynomial>
            {
                new Polynomial(new List<RationalNumber> {new RationalNumber(16, 27)}, _xDomain),
                new Polynomial(new List<RationalNumber> {4}, _xDomain),
                new Polynomial(new List<RationalNumber> {new RationalNumber(-4, 3)}, _xDomain),
                new Polynomial(new List<RationalNumber> {new RationalNumber(-8, 9)}, _xDomain),
                new Polynomial(new List<RationalNumber> {new RationalNumber(8, 9), new RationalNumber(-8, 9)},
                    _xDomain),
                new Polynomial(new List<RationalNumber> {6}, _xDomain),
                new Polynomial(new List<RationalNumber> {-2, 6}, _xDomain),
                new Polynomial(new List<RationalNumber> {-1, -2, 3}, _xDomain),
                new Polynomial(new List<RationalNumber> {1, -1, -1, 1}, _xDomain)
            };

            CollectionAssert.AreEquivalent(expected, saturatedSystem);
        }
    }
}