using System;
using System.Collections.Generic;
using System.Numerics;
using LogicLanguageLib.Alphabet;
using LogicLanguageLib.Words;

namespace LogicLanguageLib.IO
{
    public static class Parser
    {
        private static Formula ToFormula(IEnumerable<Symbol> symbols)
        {
            CheckOrder(symbols);
            var rpn = ToRpn(symbols);

            return Calc(rpn);
        }

        private static void CheckOrder(IEnumerable<Symbol> symbols)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Symbol> ToRpn(IEnumerable<Symbol> symbols)
        {
            throw new NotImplementedException();
        }

        private static Formula Calc(IEnumerable<Symbol> rpn)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Symbol> ToSymbols(string str)
        {
            var index = 0;
            var isLastForUnaryMinus = true;
            while (index < str.Length)
            {
                switch (str[index])
                {
                    case ' ':
                        continue;
                    case '(':
                        yield return LeftBracket.GetInstance();
                        isLastForUnaryMinus = true;
                        ++index;
                        continue;
                    case ')':
                        yield return RightBracket.GetInstance();
                        isLastForUnaryMinus = false;
                        ++index;
                        continue;
                    case ',':
                        yield return Comma.GetInstance();
                        isLastForUnaryMinus = true;
                        ++index;
                        continue;
                    case '\\':
                        ++index;
                        yield return SpecialSymbol(str, ref index);
                        isLastForUnaryMinus = true;
                        continue;
                }

                if (char.IsLetter(str[index]))
                {
                    yield return LetterSymbol(str, ref index);
                    isLastForUnaryMinus = false;
                    continue;
                }

                if (char.IsDigit(str[index]))
                {
                    yield return DigitSymbol(str, ref index);
                    isLastForUnaryMinus = false;
                    continue;
                }

                if (IsArithmeticPredicate(str[index]))
                {
                    yield return ArithmeticPredicateSymbol(str, ref index);
                    isLastForUnaryMinus = true;
                    continue;
                }

                if (str[index] == '-')
                {
                    if (isLastForUnaryMinus)
                    {
                        yield return UnaryMinus.GetInstance();
                        continue;
                    }
                }

                if (IsArithmeticFunction(str[index]))
                {
                    yield return SpecialFunctionSymbol(str, ref index);
                    isLastForUnaryMinus = true;
                    continue;
                }

                throw new ArgumentException($"the symbol {str[index]} with the number {index} is unexpected");
            }
        }

        private static Symbol SpecialSymbol(string str, ref int index)
        {
            ++index;
            if (index >= str.Length)
                throw new ArgumentException(
                    $"a tag was expected, but there is no tag before the symbol \\ with the number {index - 1}");

            var tag = GetTag(str, ref index);

            if (tag.Length == 0)
                throw new ArgumentException(
                    $"a tag was expected, but there is no tag before the symbol \\ with the number {index - 1}");

            return tag switch
            {
                "lnot" => (Symbol) Negation.GetInstance(),
                "lor" => Disjunction.GetInstance(),
                "land" => Conjunction.GetInstance(),
                "to" => Implication.GetInstance(),
                "forall" => UniversalQuantifier.GetInstance(),
                "exists" => ExistentialQuantifier.GetInstance(),
                "over" => Division.GetInstance(),
                _ => throw new ArgumentException($"unknown tag before the symbol with the number {index}")
            };
        }

        private static string GetTag(string str, ref int index)
        {
            var len = 0;
            while (index < str.Length && (char.IsLetter(str[index + len]) || char.IsDigit(str[index + len])))
                ++len;

            var res = str.Substring(index, len);
            index += len;

            return res;
        }

        private static Symbol LetterSymbol(string str, ref int index)
        {
            var letter = str[index];
            ++index;
            int? integer = null;
            if (index < str.Length && str[index] == '_')
            {
                ++index;
                if (index < str.Length && char.IsDigit(str[index]))
                {
                    var bigInteger = GetInteger(str, ref index);
                    if (bigInteger > int.MaxValue)
                        throw new ArgumentException(
                            $"the index must be of the int type, see before symbol with the number {index}");
                    integer = (int) bigInteger;
                }
                else
                    throw new ArgumentException($"expected number after the _ symbol with the number {index}");
            }

            return new ObjectVariable(letter, integer);
        }

        private static Symbol DigitSymbol(string str, ref int index)
        {
            return new IndividualConstant<BigInteger>(GetInteger(str, ref index));
        }

        private static BigInteger GetInteger(string str, ref int index)
        {
            var len = 0;
            while (char.IsDigit(str[index + len]))
                ++len;

            var res = BigInteger.Parse(str.Substring(index, len));
            index += len;

            return res;
        }

        private static bool IsArithmeticPredicate(char symbol)
        {
            return ArithmeticPredicate.IsArithmeticPredicate(symbol);
        }

        private static Symbol ArithmeticPredicateSymbol(string str, ref int index)
        {
            var res = ArithmeticPredicate.Factory(str[index]);
            ++index;

            return res;
        }

        private static bool IsArithmeticFunction(char symbol)
        {
            return ArithmeticBinaryFunction.IsArithmeticBinaryFunction(symbol);
        }

        private static Symbol SpecialFunctionSymbol(string str, ref int index)
        {
            var res = ArithmeticBinaryFunction.Factory(str[index]);
            ++index;

            return res;
        }

        public static Formula ToFormula(string str)
        {
            return ToFormula(ToSymbols(str));
        }
    }
}