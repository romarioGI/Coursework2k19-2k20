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
            var rpn = ToRpn(symbols);

            return Calc(rpn);
        }

        //TODO убрать "фичу" что можно не дописывать закрывающие скобки
        private static IEnumerable<Symbol> ToRpn(IEnumerable<Symbol> symbols)
        {
            Symbol previous = null;
            var stack = new Stack<Symbol>();
            foreach (var symbol in symbols)
            {
                //TODO
                CheckOrder(previous, symbol);

                if (symbol.Priority < 0)
                    yield return symbol;
                else if (symbol.Priority == 0)
                    stack.Push(symbol);
                else
                {
                    while (stack.Count > 0 && stack.Peek().Priority >= symbol.Priority)
                        yield return stack.Pop();

                    if (symbol.Priority != 10)
                        stack.Push(symbol);
                    else
                    {
                        if(symbol is RightBracket)
                            if (stack.Count == 0)
                                throw new ArgumentException("the left bracket was expected");
                            else
                                stack.Pop();
                    }
                }

                previous = symbol;
            }

            while (stack.Count > 0)
                yield return stack.Pop();
        }

        private static void CheckOrder(Symbol left, Symbol right)
        {
            var err = left switch
            {
                Comma _ => (right is Comma),
                ArithmeticBinaryFunction _ => (right is ArithmeticBinaryFunction),
                ArithmeticPredicate _ => (right is ArithmeticPredicate),
                ObjectVariable _ => (right is ObjectVariable),
                IndividualConstant<BigInteger> _ => (right is IndividualConstant<BigInteger>),
                Quantifier _ => (right is Quantifier),
                BinaryPropositionalConnective _ => (right is BinaryPropositionalConnective),
                _ => false
            };

            if (err)
                throw new ArgumentException($"symbols {left} and {right} cannot be placed side by side");
        }

        private static Formula Calc(IEnumerable<Symbol> rpn)
        {
            var stack = new Stack<object>();
            foreach (var symbol in rpn)
            {
                switch (symbol)
                {
                    case TechnicalSymbol _:
                        continue;
                    case ObjectVariable objectVariable:
                        stack.Push(new ObjectVariableTerm(objectVariable));
                        break;
                    case IndividualConstant<int> constant:
                        stack.Push(new IndividualConstantTerm<int>(constant));
                        break;
                    case IndividualConstant<BigInteger> constant:
                        stack.Push(new IndividualConstantTerm<BigInteger>(constant));
                        break;
                    case BinaryPropositionalConnective connective:
                        CalcBinaryPropositionalConnective(stack, connective);
                        break;
                    case UnaryPropositionalConnective connective:
                        CalcUnaryPropositionalConnective(stack, connective);
                        break;
                    case Quantifier quantifier:
                        CalcQuantifier(quantifier, stack);
                        break;
                    case Predicate predicate:
                        CalcPredicate(predicate, stack);
                        break;
                    case Function function:
                        CalcFunction(function, stack);
                        break;
                    default:
                        throw new NotSupportedException($"unknown symbol {symbol}");
                }
            }

            if (stack.Count != 1)
                throw new ArgumentException("incorrect formula");

            try
            {
                return (Formula) stack.Peek();
            }
            catch (Exception)
            {
                throw new ArgumentException("is not formula");
            }
        }

        private static void CalcBinaryPropositionalConnective(Stack<object> stack, BinaryPropositionalConnective connective)
        {
            if (stack.Count < 2)
                throw new ArgumentException($"two formulas were expected for {connective}");
            try
            {
                var right = (Formula) stack.Pop();
                var left = (Formula) stack.Pop();

                var newFormula = new PropositionalConnectiveFormula(connective, left, right);

                stack.Push(newFormula);
            }
            catch (Exception)
            {
                throw new ArgumentException($"two formulas were expected for {connective}");
            }
        }

        private static void CalcUnaryPropositionalConnective(Stack<object> stack, UnaryPropositionalConnective connective)
        {
            if (stack.Count < 1)
                throw new ArgumentException($"one formula was expected for {connective}");
            try
            {
                var right = (Formula) stack.Pop();

                var newFormula = new PropositionalConnectiveFormula(connective, right);

                stack.Push(newFormula);
            }
            catch (Exception)
            {
                throw new ArgumentException($"one formula was expected for {connective}");
            }
        }

        private static void CalcPredicate(Predicate predicate, Stack<object> stack)
        {
            var args = new Term[predicate.Arity];
            if (stack.Count < predicate.Arity)
                throw new ArgumentException(
                    $"expected {predicate.Arity} terms for the predicate {predicate}");
            try
            {
                for (var i = args.Length - 1; i >= 0; i--)
                    args[i] = (Term)stack.Pop();
                var newFormula = new PredicateFormula(predicate, args);

                stack.Push(newFormula);
            }
            catch (Exception)
            {
                throw new ArgumentException(
                    $"expected {predicate.Arity} terms for the predicate {predicate}");
            }
        }

        private static void CalcQuantifier(Quantifier quantifier, Stack<object> stack)
        {
            if (stack.Count < 2)
                throw new ArgumentException("incorrect quantifier formula");
            try
            {
                var formula = (Formula) stack.Pop();
                var objectVar = (ObjectVariableTerm) stack.Pop();

                var newFormula = new QuantifierFormula(quantifier, objectVar, formula);
                stack.Push(newFormula);
            }
            catch (Exception)
            {
                throw new ArgumentException("incorrect quantifier formula");
            }
        }

        private static void CalcFunction(Function function, Stack<object> stack)
        {
            var args = new Term[function.Arity];
            if (stack.Count < function.Arity)
                throw new ArgumentException(
                    $"expected {function.Arity} terms for the function {function}");
            try
            {
                for (var i = args.Length - 1; i >= 0; i--)
                    args[i] = (Term)stack.Pop();
                var newTerm = new FunctionTerm(function, args);
                stack.Push(newTerm);
            }
            catch (Exception)
            {
                throw new ArgumentException(
                    $"expected {function.Arity} terms for the predicate {function}");
            }
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
                        ++index;
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
                        ++index;
                        continue;
                    }
                }

                if (IsArithmeticBinaryFunction(str[index]))
                {
                    yield return ArithmeticBinaryFunctionSymbol(str, ref index);
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
                "lnot" => Negation.GetInstance(),
                "lor" => Disjunction.GetInstance(),
                "land" => Conjunction.GetInstance(),
                "to" => Implication.GetInstance(),
                "forall" => UniversalQuantifier.GetInstance(),
                "exists" => ExistentialQuantifier.GetInstance(),
                "over" => Division.GetInstance(),
                "func" => GetFunction(str, ref index), // \func{name arity}
                "pr" => GetPredicate(str, ref index), // \pr{name arity}
                _ => throw new ArgumentException($"unknown tag {tag} before the symbol with the number {index}")
            };
        }

        private static Function GetFunction(string str, ref int index)
        {
            var (name, arity) = GetFunctionAndPredicateParameters(str, ref index);
            return new Function(name, arity);
        }

        private static Predicate GetPredicate(string str, ref int index)
        {
            var (name, arity) = GetFunctionAndPredicateParameters(str, ref index);
            return new Predicate(name, arity);
        }

        private static (string, int) GetFunctionAndPredicateParameters(string str, ref int index)
        {
            if (index >= str.Length || str[index] != '{')
                throw new ArgumentException($"expected {{ at position {index}");
            ++index;

            var name = GetTag(str, ref index);
            if (name.Length == 0)
                throw new ArgumentException($"expected name at position {index}");

            if (index >= str.Length || str[index] != ' ')
                throw new ArgumentException($"expected whitespace at position {index}");
            ++index;

            var arity = GetInteger(str, ref index);
            if (arity > int.MaxValue)
                throw new ArgumentException($"arity should be less than {int.MaxValue}");

            if (index >= str.Length || str[index] != '}')
                throw new ArgumentException($"expected {{ at position {index}");
            ++index;

            return (name, (int) arity);
        }

        private static string GetTag(string str, ref int index)
        {
            var len = 0;
            while (index + len < str.Length && (char.IsLetter(str[index + len]) || char.IsDigit(str[index + len])))
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
            while (index + len < str.Length && char.IsDigit(str[index + len]))
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

        private static bool IsArithmeticBinaryFunction(char symbol)
        {
            return ArithmeticBinaryFunction.IsArithmeticBinaryFunction(symbol);
        }

        private static Symbol ArithmeticBinaryFunctionSymbol(string str, ref int index)
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