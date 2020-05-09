using System;
using System.Collections.Generic;
using System.Linq;
using LogicLanguageLib;
using SimpleTarskiAlgorithmLib;

namespace SimpleTarskiAlgorithmRunner
{
    public static class SimpleTarskiAlgorithm
    {
        public static Formula QuantifiersElimination(Formula formula)
        {
            switch (formula)
            {
                case FormulaPredicate _:
                    return formula;
                case FormulaPropositionalConnective formulaPropositionalConnective:
                {
                    var subFormulas = formulaPropositionalConnective.SubFormulas;
                    var eliminatedSubFormulas = subFormulas.AsParallel().Select(QuantifiersElimination).ToArray();

                    return new FormulaPropositionalConnective(
                        formulaPropositionalConnective.Connective,
                        eliminatedSubFormulas);
                }
                case FormulaQuantifier formulaQuantifier:
                {
                    if (!formulaQuantifier.IsSentence)
                        throw new ArgumentException("the algorithm does not support this formula");

                    var subFormula = formulaQuantifier.SubFormula;
                    var eliminatedSubFormula = QuantifiersElimination(subFormula);

                    return TarskiEliminate(eliminatedSubFormula, formulaQuantifier.Quantifier);
                }
                default:
                    throw new NotImplementedException();
            }
        }

        private static Formula TarskiEliminate(Formula formula, Quantifier quantifier)
        {
            var predicates = GetFormulasPredicate(formula).Distinct();

            var (expectedSigns, formulaPredicateToPolynomials) =
                ExpectedSignAndFormulaPredicateToPolynomials(predicates);

            var saturatedSystem = Saturator.Saturate(formulaPredicateToPolynomials.Values);
            var tarskiTable = new TarskiTable(saturatedSystem);
            var tarskiTableWidth = tarskiTable.Width;
            var tarskiTableDictionary = tarskiTable.GetTableDictionary();

            var newFormulas = GetNewFormulas(formula, tarskiTableWidth, expectedSigns,
                tarskiTableDictionary, formulaPredicateToPolynomials);

            return JoinFormulas(newFormulas, quantifier);
        }

        private static (Dictionary<FormulaPredicate, Sign>, Dictionary<FormulaPredicate, Polynomial>)
            ExpectedSignAndFormulaPredicateToPolynomials(IEnumerable<FormulaPredicate> predicates)
        {
            var expectedSign = new Dictionary<FormulaPredicate, Sign>();
            var formulaPredicateToPolynomials = new Dictionary<FormulaPredicate, Polynomial>();
            foreach (var formulaPredicate in predicates)
            {
                var (polynomial, sign) = ToPolynomialAndSign(formulaPredicate);
                expectedSign.Add(formulaPredicate, sign);
                formulaPredicateToPolynomials.Add(formulaPredicate, polynomial);
            }

            return (expectedSign, formulaPredicateToPolynomials);
        }

        private static IEnumerable<FormulaPredicate> GetFormulasPredicate(Formula formula)
        {
            switch (formula)
            {
                case FormulaPredicate formulaPredicate:
                {
                    yield return formulaPredicate;
                    yield break;
                }
                case FormulaPropositionalConnective formulaPropositionalConnective:
                {
                    var subFormulas = formulaPropositionalConnective.SubFormulas;
                    foreach (var subFormula in subFormulas)
                    foreach (var f in GetFormulasPredicate(subFormula))
                        yield return f;

                    yield break;
                }
                case FormulaQuantifier formulaQuantifier:
                {
                    var subFormula = formulaQuantifier.SubFormula;
                    foreach (var f in GetFormulasPredicate(subFormula))
                        yield return f;

                    yield break;
                }
                default:
                    throw new NotImplementedException();
            }
        }

        private static (Polynomial, Sign) ToPolynomialAndSign(FormulaPredicate formulaPredicate)
        {
            return FormulaParser.ToPolynomialAndSign(formulaPredicate);
        }

        private static IEnumerable<Formula> GetNewFormulas(Formula formula, int tarskiTableWidth,
            Dictionary<FormulaPredicate, Sign> expectedSigns,
            Dictionary<Polynomial, List<Sign>> tarskiTableDictionary,
            Dictionary<FormulaPredicate, Polynomial> formulaPredicateToPolynomials)
        {
            for (var i = 0; i < tarskiTableWidth; i++)
            {
                var substitutions = new Dictionary<FormulaPredicate, Predicate>();
                foreach (var (formulaPredicate, sign) in expectedSigns)
                {
                    var actualSign = tarskiTableDictionary[formulaPredicateToPolynomials[formulaPredicate]][i];
                    if (actualSign == sign)
                        substitutions.Add(formulaPredicate, True.GetInstance());
                    else
                        substitutions.Add(formulaPredicate, False.GetInstance());
                }

                yield return SubstituteInFormula(formula, substitutions);
            }
        }

        private static Formula SubstituteInFormula(Formula formula,
            Dictionary<FormulaPredicate, Predicate> substitutions)
        {
            switch (formula)
            {
                case FormulaPredicate formulaPredicate:
                {
                    var predicate = substitutions[formulaPredicate];
                    var terms = formulaPredicate.Terms.ToArray();

                    return new FormulaPredicate(predicate, terms);
                }
                case FormulaPropositionalConnective formulaPropositionalConnective:
                {
                    var connective = formulaPropositionalConnective.Connective;
                    var newSubFormulas = formulaPropositionalConnective
                        .SubFormulas
                        .AsParallel()
                        .Select(f => SubstituteInFormula(f, substitutions))
                        .ToArray();

                    return new FormulaPropositionalConnective(connective, newSubFormulas);
                }
                case FormulaQuantifier formulaQuantifier:
                {
                    var quantifier = formulaQuantifier.Quantifier;
                    var objectVariable = formulaQuantifier.ObjectVariable;
                    var newSubFormula = SubstituteInFormula(formulaQuantifier.SubFormula, substitutions);

                    return new FormulaQuantifier(quantifier, objectVariable, newSubFormula);
                }
                default:
                    throw new NotImplementedException();
            }
        }

        private static Formula JoinFormulas(IEnumerable<Formula> formulas, Quantifier quantifier)
        {
            PropositionalConnective connective = quantifier switch
            {
                ExistentialQuantifier _ => Disjunction.GetInstance(),
                UniversalQuantifier _ => Conjunction.GetInstance(),
                _ => throw new NotImplementedException()
            };

            return FormulaParser.ToFormula(formulas, connective);
        }
    }
}