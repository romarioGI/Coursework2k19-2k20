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

            if (!Predicates.Interpretations.ContainsKey(predicate))
                throw new NotSupportedException("unknown predicate");

            var sign = Predicates.Interpretations[predicate];

            var polynomial = predicate.Arity switch
            {
                2 => ToPolynomial(terms[0], variableDomain) - ToPolynomial(terms[1], variableDomain),
                1 => ToPolynomial(terms[0], variableDomain),
                _ => throw new NotSupportedException("do not support predicate with arity more then 2")
            };

            return (polynomial, sign);
        }

        public static Polynomial ToPolynomial(Term term, VariableDomain variableDomain)
        {
            switch (term)
            {
                case ObjectVariableTerm termObjectVariable:
                    var thisVariableDomain = new VariableDomain(termObjectVariable.ObjectVariable.ToString());
                    if (!variableDomain.Equals(thisVariableDomain))
                        throw new ArgumentException("VariableDomainException");

                    return new Polynomial(new List<RationalNumber> {0, 1}, variableDomain);

                case FunctionTerm termFunction:
                    var function = termFunction.Function;
                    var terms = termFunction.Terms.ToArray();

                    return TermFunctionInterpret(variableDomain, function, terms);

                case IndividualConstantTerm<int> termFunction:
                    return new Polynomial(new List<RationalNumber> {termFunction.IndividualConstant.Value},
                        variableDomain);

                case IndividualConstantTerm<RationalNumber> termFunction:
                    return new Polynomial(new List<RationalNumber> {termFunction.IndividualConstant.Value},
                        variableDomain);

                default:
                    throw new NotSupportedException("not supported term type");
            }
        }

        private static Polynomial TermFunctionInterpret(VariableDomain variableDomain, Function function, Term[] terms)
        {
            if (function is ArithmeticFunction)
            {
                var p1 = ToPolynomial(terms[0], variableDomain);
                var p2 = ToPolynomial(terms[1], variableDomain);
                if (function.Equals(Functions.Add))
                    return p1 + p2;

                if (function.Equals(Functions.Subtract))
                    return p1 - p2;

                if (function.Equals(Functions.Multi))
                    return p1 - p2;

                if (function.Equals(Functions.Divide))
                {
                    if (p2.Degree > 1)
                        throw new NotSupportedException("not support polynomial divide");

                    return p1 / p2;
                }

                if (function.Equals(Functions.Pow))
                {
                    if (p2.Degree > 1)
                        throw new NotSupportedException("not support polynomial pow");
                    if (!p2.Leading.IsNatural)
                        throw new NotSupportedException("not support not natural pow");

                    return p1.Pow(p2.Leading);
                }

                throw new NotSupportedException("not supported arithmetic function");
            }

            if (function.Equals(Functions.UnaryMinus))
            {
                var p1 = ToPolynomial(terms[0], variableDomain);

                return -p1;
            }

            throw new NotSupportedException("not supported function");
        }
    }
}