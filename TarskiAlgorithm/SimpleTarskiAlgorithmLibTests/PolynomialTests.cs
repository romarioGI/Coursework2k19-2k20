using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTarskiAlgorithmLib;
using SimpleTarskiAlgorithmLib.Exceptions;
// ReSharper disable UnusedVariable
// ReSharper disable InconsistentNaming

namespace SimpleTarskiAlgorithmLibTests
{
    [TestClass]
    public class PolynomialTests
    {
        private static readonly VariableName XName = new VariableName("x");
        private static readonly VariableName YName = new VariableName("y");

        [TestMethod]
        public void ConstructorTests()
        {
            var p1 = new Polynomial(new RationalNumber[] { }, XName);
            var p2 = new Polynomial(new RationalNumber[] {1}, XName);
            var p3 = new Polynomial(new RationalNumber[] {1, 1}, XName);
            var p4 = new Polynomial(GetRepeatCoefficients(0, 100), YName);
            var p5 = new Polynomial(GetRepeatCoefficients(new RationalNumber(1, 2), 3), YName);

            try
            {
                var p6 = new Polynomial(null, XName);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Value cannot be null. (Parameter 'coefficients')", ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
        }

        private static IEnumerable<RationalNumber> GetRepeatCoefficients(RationalNumber num, int cnt)
        {
            while (cnt-- > 0)
                yield return num;
        }

        [TestMethod]
        public void DegreeTests()
        {
            var p0 = new Polynomial(new RationalNumber[] { }, YName);
            Assert.AreEqual(-1, p0.Degree);

            var p1 = new Polynomial(GetRepeatCoefficients(new RationalNumber(5, 7), 10), YName);
            Assert.AreEqual(9, p1.Degree);

            var p2 = new Polynomial(GetRepeatCoefficients(new RationalNumber(0, 1), 100), YName);
            Assert.AreEqual(-1, p2.Degree);

            var p3 = new Polynomial(new RationalNumber[] {1, 1, 0, 5, 0, 0, 0}, YName);
            Assert.AreEqual(3, p3.Degree);
        }

        [TestMethod]
        public void LeadingTests()
        {
            var p1 = new Polynomial(new RationalNumber[] {1}, XName);
            Assert.AreEqual(1, p1.Leading);

            var p2 = new Polynomial(new RationalNumber[] {1, -10}, XName);
            Assert.AreEqual(-10, p2.Leading);

            var p3 = new Polynomial(new RationalNumber[] {1, -120, 0, 0}, YName);
            Assert.AreEqual(-120, p3.Leading);

            var p4 = new Polynomial(GetRepeatCoefficients(new RationalNumber(1, 2), 3), YName);
            Assert.AreEqual(new RationalNumber(1, 2), p4.Leading);


            var p5 = new Polynomial(new RationalNumber[] { }, XName);
            Assert.AreEqual(0, p5.Leading);

            var p6 = new Polynomial(new RationalNumber[] {0, 0, 0}, YName);
            Assert.AreEqual(0, p6.Leading);
        }

        [TestMethod]
        public void AddTests()
        {
            var p1 = new Polynomial(new RationalNumber[] {1}, XName);
            var p2 = new Polynomial(new RationalNumber[] {1}, XName);
            var expected_p1_p2 = new Polynomial(new RationalNumber[] {2}, XName);

            Assert.AreEqual(expected_p1_p2, p1 + p2);
            Assert.AreEqual(expected_p1_p2, p2 + p1);


            var p3 = new Polynomial(new RationalNumber[] {7, -3, 5}, XName);
            var p4 = new Polynomial(new RationalNumber[] {15, -13, 0, 3, 0, -6, 32}, XName);
            var expected_p3_p4 = new Polynomial(new RationalNumber[] {22, -16, 5, 3, 0, -6, 32}, XName);

            Assert.AreEqual(expected_p3_p4, p3 + p4);
            Assert.AreEqual(expected_p3_p4, p4 + p3);


            var p5 = new Polynomial(GetRepeatCoefficients(new RationalNumber(1, 3), 5), XName);
            var p6 = new Polynomial(GetRepeatCoefficients(new RationalNumber(2, 3), 5), XName);
            var expected_p5_p6 = new Polynomial(GetRepeatCoefficients(1, 5), XName);

            Assert.AreEqual(expected_p5_p6, p5 + p6);
            Assert.AreEqual(expected_p5_p6, p6 + p5);


            var p7 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, YName);
            var p8 = new Polynomial(new RationalNumber[] {1, 2, 0, -1}, YName);
            var expected_p7_p8 = new Polynomial(new RationalNumber[] {2, 4}, YName);

            Assert.AreEqual(expected_p7_p8, p7 + p8);
            Assert.AreEqual(expected_p7_p8, p8 + p7);


            var p9 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, YName);
            var p10 = new Polynomial(new RationalNumber[] {0}, YName);
            var expected_p9_p10 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, YName);

            Assert.AreEqual(expected_p9_p10, p9 + p10);
            Assert.AreEqual(expected_p9_p10, p10 + p9);

            try
            {
                var p11 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, XName);
                var p12 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, YName);
                var _ = p11 + p12;
                Assert.Fail("An exception should have been thrown");
            }
            catch (PolynomialObjectVariableException ae)
            {
                Assert.AreEqual($"Polynomials have {XName} and {YName} VariableNames, but they must be equal",
                    ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
        }

        [TestMethod]
        public void SubTests()
        {
            var p1 = new Polynomial(new RationalNumber[] {1}, XName);
            var p2 = new Polynomial(new RationalNumber[] {-1}, XName);
            var expected_p1_p2 = new Polynomial(new RationalNumber[] {2}, XName);

            Assert.AreEqual(expected_p1_p2, p1 - p2);


            var p3 = new Polynomial(new RationalNumber[] {7, -3, 5}, XName);
            var p4 = new Polynomial(new RationalNumber[] {15, -13, 0, 3, 0, -6, 32}, XName);
            var expected_p3_p4 = new Polynomial(new RationalNumber[] {-8, 10, 5, -3, 0, 6, -32}, XName);

            Assert.AreEqual(expected_p3_p4, p3 - p4);


            var p5 = new Polynomial(GetRepeatCoefficients(new RationalNumber(5, 3), 5), XName);
            var p6 = new Polynomial(GetRepeatCoefficients(new RationalNumber(2, 3), 5), XName);
            var expected_p5_p6 = new Polynomial(GetRepeatCoefficients(1, 5), XName);

            Assert.AreEqual(expected_p5_p6, p5 - p6);


            var p7 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, YName);
            var p8 = new Polynomial(new RationalNumber[] {1, -2, 0, 1}, YName);
            var expected_p7_p8 = new Polynomial(new RationalNumber[] {0, 4}, YName);

            Assert.AreEqual(expected_p7_p8, p7 - p8);


            var p9 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, YName);
            var p10 = new Polynomial(new RationalNumber[] {0}, YName);
            var expected_p9_p10 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, YName);

            Assert.AreEqual(expected_p9_p10, p9 - p10);

            var expected_p10_p9 = new Polynomial(new RationalNumber[] {-1, -2, 0, -1}, YName);

            Assert.AreEqual(expected_p10_p9, p10 - p9);

            try
            {
                var p11 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, XName);
                var p12 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, YName);
                var _ = p11 - p12;
                Assert.Fail("An exception should have been thrown");
            }
            catch (PolynomialObjectVariableException ae)
            {
                Assert.AreEqual($"Polynomials have {XName} and {YName} VariableNames, but they must be equal",
                    ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
        }

        [TestMethod]
        public void MultiTests()
        {
            var p1 = new Polynomial(new RationalNumber[] {1}, XName);
            var p2 = new Polynomial(new RationalNumber[] {-1}, XName);
            var expected_p1_p2 = new Polynomial(new RationalNumber[] {-1}, XName);

            Assert.AreEqual(expected_p1_p2, p1 * p2);
            Assert.AreEqual(expected_p1_p2, p2 * p1);


            var p3 = new Polynomial(new RationalNumber[] {7, -3, 5}, XName);
            var p4 = new Polynomial(new RationalNumber[] {15, -13, 0, 3, 0, -6, 32}, XName);
            var expected_p3_p4 = new Polynomial(new RationalNumber[] {105, -136, 114, -44, -9, -27, 242, -126, 160},
                XName);

            Assert.AreEqual(expected_p3_p4, p3 * p4);
            Assert.AreEqual(expected_p3_p4, p4 * p3);


            var p5 = new Polynomial(GetRepeatCoefficients(new RationalNumber(3, 2), 3), XName);
            var p6 = new Polynomial(GetRepeatCoefficients(new RationalNumber(2, 3), 3), XName);
            var expected_p5_p6 = new Polynomial(new RationalNumber[] {1, 2, 3, 2, 1}, XName);

            Assert.AreEqual(expected_p5_p6, p5 * p6);
            Assert.AreEqual(expected_p5_p6, p6 * p5);


            var p9 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, YName);
            var p10 = new Polynomial(new RationalNumber[] {0}, YName);
            var expected_p9_p10 = new Polynomial(new RationalNumber[] {0}, YName);

            Assert.AreEqual(expected_p9_p10, p9 * p10);
            Assert.AreEqual(expected_p9_p10, p10 * p9);

            try
            {
                var p11 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, XName);
                var p12 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, YName);
                var _ = p11 * p12;
                Assert.Fail("An exception should have been thrown");
            }
            catch (PolynomialObjectVariableException ae)
            {
                Assert.AreEqual($"Polynomials have {XName} and {YName} VariableNames, but they must be equal",
                    ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
        }

        [TestMethod]
        public void MultiToIntTests()
        {
            var p1 = new Polynomial(new RationalNumber[] {1}, XName);
            var p2 = -1;
            var expected_p1_p2 = new Polynomial(new RationalNumber[] {-1}, XName);

            Assert.AreEqual(expected_p1_p2, p1 * p2);
            Assert.AreEqual(expected_p1_p2, p2 * p1);


            var p3 = new Polynomial(new RationalNumber[] {7, -3, 5}, XName);
            var p4 = 3;
            var expected_p3_p4 = new Polynomial(new RationalNumber[] {21, -9, 15},
                XName);

            Assert.AreEqual(expected_p3_p4, p3 * p4);
            Assert.AreEqual(expected_p3_p4, p4 * p3);


            var p5 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, YName);
            var p6 = 0;
            var expected_p5_p6 = new Polynomial(new RationalNumber[] {0}, YName);

            Assert.AreEqual(expected_p5_p6, p5 * p6);
            Assert.AreEqual(expected_p5_p6, p5 * p6);
        }

        [TestMethod]
        public void DivTests()
        {
            var p1 = new Polynomial(new RationalNumber[] {1}, XName);
            var p2 = new Polynomial(new RationalNumber[] {1}, XName);
            var expected_p1_p2 = new Polynomial(new RationalNumber[] {1}, XName);

            Assert.AreEqual(expected_p1_p2, p1 / p2);


            var p3 = new Polynomial(new RationalNumber[] {7, -3, 5}, XName);
            var p4 = new Polynomial(new RationalNumber[] {15, -13, 0, 3, 0, -6, 32}, XName);
            var expected_p3_p4 = new Polynomial(new RationalNumber[] {0}, XName);
            var expected_p4_p3 = new Polynomial(new []
            {
                new RationalNumber(18167, 3125),
                new RationalNumber(-4701, 625),
                new RationalNumber(-922, 125),
                new RationalNumber(66, 25),
                new RationalNumber(32, 5),
            }, XName);

            Assert.AreEqual(expected_p3_p4, p3 / p4);
            Assert.AreEqual(expected_p4_p3, p4 / p3);


            var p5 = new Polynomial(GetRepeatCoefficients(new RationalNumber(2, 3), 5), XName);
            var p6 = new Polynomial(GetRepeatCoefficients(new RationalNumber(1, 3), 5), XName);
            var expected_p5_p6 = new Polynomial(new RationalNumber[] {2}, XName);

            Assert.AreEqual(expected_p5_p6, p5 / p6);


            var p7 = new Polynomial(new RationalNumber[] {0}, YName);
            var p8 = new Polynomial(new RationalNumber[] {1, 2, 0, -1}, YName);
            var expected_p7_p8 = new Polynomial(new RationalNumber[] {0}, YName);

            Assert.AreEqual(expected_p7_p8, p7 / p8);

            try
            {
                var p11 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, XName);
                var p12 = new Polynomial(new RationalNumber[] {0}, XName);
                var _ = p11 / p12;
                Assert.Fail("An exception should have been thrown");
            }
            catch (DivideByZeroException ae)
            {
                Assert.AreEqual("Attempted to divide by zero.", ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }

            try
            {
                var p11 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, XName);
                var p12 = new Polynomial(new RationalNumber[] {1, 2, 0, 1}, YName);
                var _ = p11 / p12;
                Assert.Fail("An exception should have been thrown");
            }
            catch (PolynomialObjectVariableException ae)
            {
                Assert.AreEqual($"Polynomials have {XName} and {YName} VariableNames, but they must be equal",
                    ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
        }

        [TestMethod]
        public void ReminderTests()
        {
            var p1 = new Polynomial(new RationalNumber[] { 1 }, XName);
            var p2 = new Polynomial(new RationalNumber[] { 1 }, XName);
            var expected_p1_p2 = new Polynomial(new RationalNumber[] { 0 }, XName);

            Assert.AreEqual(expected_p1_p2, p1 % p2);


            var p3 = new Polynomial(new RationalNumber[] { 7, -3, 5 }, XName);
            var p4 = new Polynomial(new RationalNumber[] { 15, -13, 0, 3, 0, -6, 32 }, XName);
            var expected_p3_p4 = new Polynomial(new RationalNumber[] { 7, -3, 5 }, XName);
            var expected_p4_p3 = new Polynomial(new []
            {
                new RationalNumber(-80294, 3125),
                new RationalNumber(178411, 3125),
            }, XName);

            Assert.AreEqual(expected_p3_p4, p3 % p4);
            Assert.AreEqual(expected_p4_p3, p4 % p3);


            var p5 = new Polynomial(GetRepeatCoefficients(new RationalNumber(2, 3), 5), XName);
            var p6 = new Polynomial(GetRepeatCoefficients(new RationalNumber(1, 3), 5), XName);
            var expected_p5_p6 = new Polynomial(new RationalNumber[] { 0 }, XName);

            Assert.AreEqual(expected_p5_p6, p5 % p6);


            var p7 = new Polynomial(new RationalNumber[] { 0 }, YName);
            var p8 = new Polynomial(new RationalNumber[] { 1, 2, 0, -1 }, YName);
            var expected_p7_p8 = new Polynomial(new RationalNumber[] { 0 }, YName);

            Assert.AreEqual(expected_p7_p8, p7 % p8);

            try
            {
                var p9 = new Polynomial(new RationalNumber[] { 1, 2, 0, 1 }, XName);
                var p10 = new Polynomial(new RationalNumber[] { 0 }, XName);
                var _ = p9 % p10;
                Assert.Fail("An exception should have been thrown");
            }
            catch (DivideByZeroException ae)
            {
                Assert.AreEqual("Attempted to divide by zero.", ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }

            try
            {
                var p11 = new Polynomial(new RationalNumber[] { 1, 2, 0, 1 }, XName);
                var p12 = new Polynomial(new RationalNumber[] { 1, 2, 0, 1 }, YName);
                var _ = p11 % p12;
                Assert.Fail("An exception should have been thrown");
            }
            catch (PolynomialObjectVariableException ae)
            {
                Assert.AreEqual($"Polynomials have {XName} and {YName} VariableNames, but they must be equal",
                    ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
        }

        [TestMethod]
        public void EqualTests()
        {
            var p1 = new Polynomial(new RationalNumber[] { 1 }, XName);
            var p2 = new Polynomial(new RationalNumber[] { 1 }, XName);

            Assert.AreEqual(true, p1 == p2);
            Assert.AreEqual(true, p2 == p1);


            var p3 = new Polynomial(new RationalNumber[] { 7, -3, 5 }, XName);
            var p4 = new Polynomial(new RationalNumber[] { 15, -13, 0, 3, 0, -6, 32 }, XName);

            Assert.AreEqual(false, p3 == p4);
            Assert.AreEqual(false, p4 == p3);


            var p5 = new Polynomial(GetRepeatCoefficients(new RationalNumber(1, 3), 5), XName);
            var p6 = new Polynomial(GetRepeatCoefficients(new RationalNumber(2, 6), 5), XName);

            Assert.AreEqual(true, p5 == p6);
            Assert.AreEqual(true, p6 == p5);


            var p7 = new Polynomial(new RationalNumber[] { 1, 2, 0, 1 }, YName);
            var p8 = new Polynomial(new RationalNumber[] { 1, 2, 0, 1, 0, 0, 0 }, YName);

            Assert.AreEqual(true, p7 == p8);
            Assert.AreEqual(true, p8 == p7);


            var p9 = new Polynomial(new RationalNumber[] { 1, 2, 0, 1 }, YName);
            p9 -= p9;
            var p10 = new Polynomial(new RationalNumber[] { 0 }, YName);

            Assert.AreEqual(true, p9 == p10);
            Assert.AreEqual(true, p10 == p9);

            try
            {
                var p11 = new Polynomial(new RationalNumber[] { 1, 2, 0, 1 }, XName);
                var p12 = new Polynomial(new RationalNumber[] { 1, 2, 0, 1 }, YName);
                var _ = p11 == p12;
                Assert.Fail("An exception should have been thrown");
            }
            catch (PolynomialObjectVariableException ae)
            {
                Assert.AreEqual($"Polynomials have {XName} and {YName} VariableNames, but they must be equal",
                    ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
        }

        [TestMethod]
        public void GetDerivativeTests()
        {
            var p1 = new Polynomial(new RationalNumber[] { 1 }, XName);
            var expected_p1 = new Polynomial(new RationalNumber[]{}, XName);

            Assert.AreEqual(expected_p1, p1.GetDerivative());


            var p2 = new Polynomial(new RationalNumber[] { 15, 0, -13, 0, 3, 0, -6, 32 }, XName);
            var expected_p2 = new Polynomial(new RationalNumber[] { 0, -26, 0, 12, 0, -36, 224 }, XName);

            Assert.AreEqual(expected_p2, p2.GetDerivative());


            var p3 = new Polynomial(GetRepeatCoefficients(1, 5), XName);
            var expected_p3 = new Polynomial(new RationalNumber[]{1,2,3,4}, XName);

            Assert.AreEqual(expected_p3, p3.GetDerivative());


            var p4 = new Polynomial(new RationalNumber[] { 0 }, YName);
            var expected_p4 = new Polynomial(new RationalNumber[] { 0 }, YName);

            Assert.AreEqual(expected_p4, p4.GetDerivative());
        }

        [TestMethod]
        public void HashCodeTests()
        {
            var p1 = new Polynomial(new RationalNumber[] { 1 }, XName);
            var p2 = new Polynomial(new RationalNumber[] { 1 }, XName);

            Assert.IsTrue(p1.GetHashCode() == p2.GetHashCode());


            var p3 = new Polynomial(new RationalNumber[] { 7, -3, 5 }, XName);
            var p4 = new Polynomial(new RationalNumber[] { 15, -13, 0, 3, 0, -6, 32 }, XName);

            Assert.IsFalse(p3.GetHashCode() == p4.GetHashCode());


            var p5 = new Polynomial(GetRepeatCoefficients(new RationalNumber(1, 3), 5), XName);
            var p6 = new Polynomial(GetRepeatCoefficients(new RationalNumber(2, 3), 5), XName);

            Assert.IsFalse(p5.GetHashCode() == p6.GetHashCode());


            var p7 = new Polynomial(new RationalNumber[] { 1, 2, 0, 1 }, YName);
            var p8 = new Polynomial(new RationalNumber[] { 1, 2, 0, 1, 0, 0, 0 }, YName);

            Assert.IsTrue(p7.GetHashCode() == p8.GetHashCode());


            var p9 = new Polynomial(new RationalNumber[] { 1, 2, 0, 1 }, YName);
            p9 -= p9;
            var p10 = new Polynomial(new RationalNumber[] { 0 }, YName);

            Assert.IsTrue(p9.GetHashCode() == p10.GetHashCode());


            var p11 = new Polynomial(new RationalNumber[]{}, XName);
            var p12 = new Polynomial(new RationalNumber[]{0}, YName);

            Assert.IsFalse(p11.GetHashCode() == p12.GetHashCode());
        }
    }
}