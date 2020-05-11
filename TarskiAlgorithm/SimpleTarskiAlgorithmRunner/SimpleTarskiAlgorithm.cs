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
                case PredicateFormula _:
                    return formula;
                case PropositionalConnectiveFormula formulaPropositionalConnective:
                {
                    var subFormulas = formulaPropositionalConnective.SubFormulas;
                    var eliminatedSubFormulas = subFormulas.Select(QuantifiersElimination).ToArray();

                    return CalcFormula(formulaPropositionalConnective.Connective, eliminatedSubFormulas);
                }
                case QuantifierFormula formulaQuantifier:
                {
                    if (!formulaQuantifier.IsSentence)
                        throw new NotSupportedException("the algorithm does not support open quantifier formulas");

                    var subFormula = formulaQuantifier.SubFormula;
                    var eliminatedSubFormula = QuantifiersElimination(subFormula);
                    var variableDomain = new VariableName(formulaQuantifier.ObjectVariable.ToString());

                    return TarskiEliminate(eliminatedSubFormula, formulaQuantifier.Quantifier, variableDomain);
                }
                default:
                    throw new NotSupportedException("not supported formula type");
            }
        }

        private static Formula CalcFormula(PropositionalConnective connective,
            params Formula[] subFormulas)
        {
            if (!subFormulas.Any(f =>
                f is PredicateFormula prF &&
                prF.Predicate is BooleanPredicate))
                return new PropositionalConnectiveFormula(connective, subFormulas);

            switch (connective)
            {
                case BinaryPropositionalConnective _:
                {
                    var f1 = subFormulas[0];
                    var f2 = subFormulas[1];

                    BooleanPredicate predicate;
                    Formula formula;
                    bool predicateIsLeft;
                    if (f1 is PredicateFormula pr1 && pr1.Predicate is BooleanPredicate booleanPredicate1)
                    {
                        predicate = booleanPredicate1;
                        formula = f2;
                        predicateIsLeft = true;
                    }
                    else if (f2 is PredicateFormula pr2 && pr2.Predicate is BooleanPredicate booleanPredicate2)
                    {
                        predicate = booleanPredicate2;
                        formula = f1;
                        predicateIsLeft = false;
                    }
                    else
                        throw new Exception();

                    formula = connective switch
                    {
                        Disjunction _ => predicate switch
                        {
                            True _ => new PredicateFormula(True.GetInstance()),
                            False _ => formula,
                            _ => throw new NotSupportedException("not supported to calc BooleanPredicate type")
                        },
                        Conjunction _ => predicate switch
                        {
                            True _ => formula,
                            False _ => new PredicateFormula(False.GetInstance()),
                            _ => throw new NotSupportedException("not supported to calc BooleanPredicate type")
                        },
                        Implication _ => predicate switch
                        {
                            True _ => predicateIsLeft ? formula : new PredicateFormula(True.GetInstance()),
                            False _ => predicateIsLeft
                                ? new PredicateFormula(True.GetInstance())
                                : CalcFormula(Negation.GetInstance(), formula),
                            _ => throw new NotSupportedException("not supported to calc BooleanPredicate type")
                        },
                        _ => throw new NotSupportedException("not supported to calc BinaryPropositionalConnective type")
                    };

                    return formula;
                }
                case UnaryPropositionalConnective _:
                {
                    var predicate = (BooleanPredicate) ((PredicateFormula) subFormulas[0]).Predicate;
                    predicate = connective switch
                    {
                        Negation _ => predicate switch
                        {
                            True _ => False.GetInstance(),
                            False _ => True.GetInstance(),
                            _ => throw new NotSupportedException("not supported to calc BooleanPredicate type")
                        },
                        _ => throw new NotSupportedException("not supported to calc UnaryPropositionalConnective type")
                    };

                    return new PredicateFormula(predicate);
                }
                default:
                    throw new NotSupportedException("not supported to calc PropositionalConnective type");
            }
        }

        private static Formula TarskiEliminate(Formula formula, Quantifier quantifier, VariableName variableName)
        {
            var predicates = GetFormulasPredicate(formula).Distinct();

            var (expectedSigns, formulaPredicateToPolynomials) =
                ExpectedSignAndFormulaPredicateToPolynomials(predicates, variableName);

            var saturatedSystem = SimpleSaturator.Saturate(formulaPredicateToPolynomials.Values);
            var tarskiTable = new SimpleTarskiTable(saturatedSystem);
            var tarskiTableWidth = tarskiTable.Width;
            var tarskiTableDictionary = tarskiTable.GetTableDictionary();

            var newFormulas = GetNewFormulas(formula, tarskiTableWidth, expectedSigns,
                tarskiTableDictionary, formulaPredicateToPolynomials);

            return JoinFormulas(newFormulas, quantifier);
        }

        private static (Dictionary<PredicateFormula, Sign>, Dictionary<PredicateFormula, Polynomial>)
            ExpectedSignAndFormulaPredicateToPolynomials(IEnumerable<PredicateFormula> predicates,
                VariableName variableName)
        {
            var expectedSign = new Dictionary<PredicateFormula, Sign>();
            var formulaPredicateToPolynomials = new Dictionary<PredicateFormula, Polynomial>();
            foreach (var formulaPredicate in predicates)
            {
                var (polynomial, sign) = ToPolynomialAndSign(formulaPredicate, variableName);
                expectedSign.Add(formulaPredicate, sign);
                formulaPredicateToPolynomials.Add(formulaPredicate, polynomial);
            }

            return (expectedSign, formulaPredicateToPolynomials);
        }

        private static IEnumerable<PredicateFormula> GetFormulasPredicate(Formula formula)
        {
            switch (formula)
            {
                case PredicateFormula formulaPredicate:
                {
                    if (formulaPredicate.Predicate.Arity != 0)
                        yield return formulaPredicate;
                    yield break;
                }
                case PropositionalConnectiveFormula formulaPropositionalConnective:
                {
                    var subFormulas = formulaPropositionalConnective.SubFormulas;
                    foreach (var subFormula in subFormulas)
                    foreach (var f in GetFormulasPredicate(subFormula))
                        yield return f;

                    yield break;
                }
                case QuantifierFormula formulaQuantifier:
                {
                    var subFormula = formulaQuantifier.SubFormula;
                    foreach (var f in GetFormulasPredicate(subFormula))
                        yield return f;

                    yield break;
                }
                default:
                    throw new NotSupportedException("not supported Formula type");
            }
        }

        private static (Polynomial, Sign) ToPolynomialAndSign(PredicateFormula predicateFormula,
            VariableName variableName)
        {
            return FormulaConverter.ToPolynomialAndSign(predicateFormula, variableName);
        }

        private static IEnumerable<Formula> GetNewFormulas(Formula formula, int tarskiTableWidth,
            Dictionary<PredicateFormula, Sign> expectedSigns,
            Dictionary<Polynomial, List<Sign>> tarskiTableDictionary,
            Dictionary<PredicateFormula, Polynomial> formulaPredicateToPolynomials)
        {
            for (var i = 0; i < tarskiTableWidth; i++)
            {
                var substitutions = new Dictionary<PredicateFormula, BooleanPredicate>();
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
            Dictionary<PredicateFormula, BooleanPredicate> substitutions)
        {
            switch (formula)
            {
                case PredicateFormula formulaPredicate:
                {
                    var predicate = substitutions[formulaPredicate];

                    return new PredicateFormula(predicate);
                }
                case PropositionalConnectiveFormula formulaPropositionalConnective:
                {
                    var connective = formulaPropositionalConnective.Connective;
                    var newSubFormulas = formulaPropositionalConnective
                        .SubFormulas
                        .AsParallel()
                        .Select(f => SubstituteInFormula(f, substitutions))
                        .ToArray();

                    return CalcFormula(connective, newSubFormulas);
                }
                default:
                    throw new NotSupportedException("not supported Formula type");
            }
        }

        private static Formula JoinFormulas(IEnumerable<Formula> formulas, Quantifier quantifier)
        {
            var connective = quantifier switch
            {
                ExistentialQuantifier _ => (PropositionalConnective) Disjunction.GetInstance(),
                UniversalQuantifier _ => Conjunction.GetInstance(),
                _ => throw new NotSupportedException("not supported Quantifier type")
            };

            var formula = formulas.Aggregate((res, f) => CalcFormula(connective, res, f));

            return formula;
        }
    }
}