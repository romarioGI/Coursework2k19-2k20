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
                    var variableDomain = new VariableDomain(formulaQuantifier.ObjectVariable.ToString());

                    return TarskiEliminate(eliminatedSubFormula, formulaQuantifier.Quantifier, variableDomain);
                }
                default:
                    throw new NotImplementedException();
            }
        }

        private static Formula TarskiEliminate(Formula formula, Quantifier quantifier, VariableDomain variableDomain)
        {
            var predicates = GetFormulasPredicate(formula).Distinct();

            var (expectedSigns, formulaPredicateToPolynomials) =
                ExpectedSignAndFormulaPredicateToPolynomials(predicates, variableDomain);

            var saturatedSystem = Saturator.Saturate(formulaPredicateToPolynomials.Values);
            var tarskiTable = new TarskiTable(saturatedSystem);
            var tarskiTableWidth = tarskiTable.Width;
            var tarskiTableDictionary = tarskiTable.GetTableDictionary();

            var newFormulas = GetNewFormulas(formula, tarskiTableWidth, expectedSigns,
                tarskiTableDictionary, formulaPredicateToPolynomials);

            return JoinFormulas(newFormulas, quantifier);
        }

        private static (Dictionary<FormulaPredicate, Sign>, Dictionary<FormulaPredicate, Polynomial>)
            ExpectedSignAndFormulaPredicateToPolynomials(IEnumerable<FormulaPredicate> predicates,
                VariableDomain variableDomain)
        {
            var expectedSign = new Dictionary<FormulaPredicate, Sign>();
            var formulaPredicateToPolynomials = new Dictionary<FormulaPredicate, Polynomial>();
            foreach (var formulaPredicate in predicates)
            {
                var (polynomial, sign) = ToPolynomialAndSign(formulaPredicate, variableDomain);
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
                    if (formulaPredicate.Predicate.Arity != 0)
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

        private static (Polynomial, Sign) ToPolynomialAndSign(FormulaPredicate formulaPredicate,
            VariableDomain variableDomain)
        {
            return FormulaConverter.ToPolynomialAndSign(formulaPredicate, variableDomain);
        }

        private static IEnumerable<FormulaPredicate> GetNewFormulas(Formula formula, int tarskiTableWidth,
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

        private static FormulaPredicate SubstituteInFormula(Formula formula,
            Dictionary<FormulaPredicate, Predicate> substitutions)
        {
            switch (formula)
            {
                case FormulaPredicate formulaPredicate:
                {
                    var predicate = substitutions[formulaPredicate];

                    return new FormulaPredicate(predicate);
                }
                case FormulaPropositionalConnective formulaPropositionalConnective:
                {
                    var trueFormula = new FormulaPredicate(True.GetInstance());
                    var falseFormula = new FormulaPredicate(False.GetInstance());

                    var connective = formulaPropositionalConnective.Connective;
                    var newSubFormulas = formulaPropositionalConnective
                        .SubFormulas
                        .AsParallel()
                        .Select(f => SubstituteInFormula(f, substitutions))
                        .ToArray();

                    switch (connective)
                    {
                        case Disjunction _:
                        {
                            var res = newSubFormulas.Any(f => f.Equals(trueFormula));
                            return res ? trueFormula : falseFormula;
                        }
                        case Conjunction _:
                        {
                            var res = newSubFormulas.All(f => f.Equals(trueFormula));
                            return res ? trueFormula : falseFormula;
                        }
                        case Implication _:
                        {
                            var res = !(newSubFormulas[0].Equals(trueFormula) &&
                                        newSubFormulas[1].Equals(falseFormula));
                            return res ? trueFormula : falseFormula;
                        }
                        case Negation _:
                        {
                            var res = !newSubFormulas.Equals(trueFormula);
                            return res ? trueFormula : falseFormula;
                        }
                        default:
                            throw new NotImplementedException();
                    }
                }
                default:
                    throw new NotImplementedException();
            }
        }

        private static Formula JoinFormulas(IEnumerable<FormulaPredicate> formulas, Quantifier quantifier)
        {
            var trueFormula = new FormulaPredicate(True.GetInstance());
            var falseFormula = new FormulaPredicate(False.GetInstance());
            switch (quantifier)
            {
                case ExistentialQuantifier _:
                {
                    var res = formulas.Any(f => f.Equals(trueFormula));

                    return res ? trueFormula : falseFormula;
                }
                case UniversalQuantifier _:
                {
                    var res = formulas.All(f => f.Equals(trueFormula));

                    return res ? trueFormula : falseFormula;
                }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}