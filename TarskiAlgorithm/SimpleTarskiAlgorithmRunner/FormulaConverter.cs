using System;
using System.Collections.Generic;
using System.Linq;
using LogicLanguageLib;
using SimpleTarskiAlgorithmLib;

namespace SimpleTarskiAlgorithmRunner
{
    public static class FormulaConverter
    {
        public static Formula ToFormula(IEnumerable<Symbol> symbols)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<Symbol> ToSymbols(string str)
        {
            throw new NotImplementedException();
        }

        public static Formula ToFormula(string str)
        {
            return ToFormula(ToSymbols(str));
        }

        public static (Polynomial, Sign) ToPolynomialAndSign(PredicateFormula predicateFormula,
            VariableDomain variableDomain)
        {
            var predicate = predicateFormula.Predicate;
            var terms = predicateFormula.Terms.ToArray();
            switch (predicate.Arity)
            {
                case 2:
                {
                    var polynomial = ToPolynomial(terms[0], variableDomain) - ToPolynomial(terms[1], variableDomain);
                    if (predicate.Equals(Predicates.Less))
                        return (polynomial, Sign.LessZero);

                    if (predicate.Equals(Predicates.Equal))
                        return (polynomial, Sign.Zero);

                    if (predicate.Equals(Predicates.More))
                        return (polynomial, Sign.MoreZero);

                    throw new NotImplementedException();
                }
                case 1:
                {
                    var polynomial = ToPolynomial(terms[0], variableDomain);
                    if (predicate.Equals(Predicates.LessZero))
                        return (polynomial, Sign.LessZero);

                    if (predicate.Equals(Predicates.EqualZero))
                        return (polynomial, Sign.Zero);

                    if (predicate.Equals(Predicates.MoreZero))
                        return (polynomial, Sign.MoreZero);

                    throw new NotImplementedException();
                }
                default:
                    throw new NotImplementedException();
            }
        }

        public static Polynomial ToPolynomial(Term term, VariableDomain variableDomain)
        {
            switch (term)
            {
                case ObjectVariableTerm termObjectVariable:
                    var thisVariableDomain = new VariableDomain(termObjectVariable.ObjectVariable.ToString());
                    if (!variableDomain.Equals(thisVariableDomain))
                        throw new ArgumentException();

                    return new Polynomial(new List<RationalNumber> {0, 1}, variableDomain);

                case FunctionTerm termFunction:
                    var function = termFunction.Function;
                    var terms = termFunction.Terms.ToArray();
                    switch (function.Arity)
                    {
                        case 2:
                        {
                            if (function.Equals(Functions.Pow))
                            {
                                var p = ToPolynomial(terms[0], variableDomain);
                                if (terms[1] is IndividualConstantTerm<int> termIndividualConstant)
                                    return p.Pow(termIndividualConstant.IndividualConstant.Value);

                                throw new NotImplementedException();
                            }

                            if (function.Equals(Functions.Pow))
                            {
                                if (terms[0] is IndividualConstantTerm<int> && terms[1] is IndividualConstantTerm<int>)
                                {
                                    var int1 = ((IndividualConstantTerm<int>) terms[0]).IndividualConstant.Value;
                                    var int2 = ((IndividualConstantTerm<int>) terms[1]).IndividualConstant.Value;

                                    return new Polynomial(new List<RationalNumber> {new RationalNumber(int1, int2)},
                                        variableDomain);
                                }

                                throw new NotImplementedException();
                            }

                            var p1 = ToPolynomial(terms[0], variableDomain);
                            var p2 = ToPolynomial(terms[1], variableDomain);
                            if (function.Equals(Functions.Add))
                                return p1 + p2;

                            if (function.Equals(Functions.Subtract))
                                return p1 - p2;

                            if (function.Equals(Functions.Multi))
                                return p1 * p2;

                            throw new NotImplementedException();
                        }
                        case 1:
                        {
                            if (function.Equals(Functions.UnaryMinus))
                            {
                                var p1 = ToPolynomial(terms[0], variableDomain);
                                return -p1;
                            }

                            throw new NotImplementedException();
                        }
                        default:
                            throw new NotImplementedException();
                    }

                case IndividualConstantTerm<int> termFunction:
                    return new Polynomial(new List<RationalNumber>() {termFunction.IndividualConstant.Value},
                        variableDomain);

                case IndividualConstantTerm<RationalNumber> termFunction:
                    return new Polynomial(new List<RationalNumber>() {termFunction.IndividualConstant.Value},
                        variableDomain);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}