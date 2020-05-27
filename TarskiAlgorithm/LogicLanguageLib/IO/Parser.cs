using System;
using System.Collections.Generic;
using System.Numerics;
using LogicLanguageLib.Alphabet;
using LogicLanguageLib.Words;

namespace LogicLanguageLib.IO
{
    public static class Parser
    {
        public static Formula ToFormula(IEnumerable<Symbol> symbols)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<Symbol> ToSymbols(string str)
        {
            var index = 0;
            while (index < str.Length)
            {
                switch (str[index])
                {
                    case ' ':
                        continue;
                    case '(':
                        yield return LeftBracket.GetInstance();
                        ++index;
                        continue;
                    case ')':
                        yield return RightBracket.GetInstance();
                        ++index;
                        continue;
                    case ',':
                        yield return Comma.GetInstance();
                        ++index;
                        continue;
                    case '\\':
                        ++index;
                        yield return SpecialSymbol(str, ref index);
                        continue;
                }

                if (char.IsLetter(str[index]))
                {
                    yield return LetterSymbol(str, ref index);
                    continue;
                }

                if (char.IsDigit(str[index]))
                {
                    yield return DigitSymbol(str, ref index);
                    continue;
                }

                if (IsSpecialPredicate(str[index]))
                {
                    yield return SpecialPredicateSymbol(str, ref index);
                    continue;
                }

                if (IsSpecialFunction(str[index]))
                {
                    yield return SpecialFunctionSymbol(str, ref index);
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
                "over" => new ArithmeticFunction("/"),
                "func" => throw new NotImplementedException(), // \func{arity} дальше через запятую параметры
                "pr" => throw new NotImplementedException(), // \pr{arity} дальше через запятую параметры
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

        private static bool IsSpecialPredicate(char symbol)
        {
            return symbol == '<' || symbol == '>' || symbol == '=';
        }

        private static Symbol SpecialPredicateSymbol(string str, ref int index)
        {
            var res = new Predicate(str[index].ToString(), 2);
            ++index;

            return res;
        }

        private static bool IsSpecialFunction(char symbol)
        {
            return symbol == '+' || symbol == '-' || symbol == '^' || symbol == '*' || symbol == '/';
        }

        private static Symbol SpecialFunctionSymbol(string str, ref int index)
        {
            var res = new ArithmeticFunction(str[index].ToString());
            ++index;

            return res;
        }

        public static Formula ToFormula(string str)
        {
            return ToFormula(ToSymbols(str));
        }
    }
}