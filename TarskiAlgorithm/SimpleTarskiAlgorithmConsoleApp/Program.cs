using System;
using LogicLanguageLib.IO;
using LogicLanguageLib.Words;
using SimpleTarskiAlgorithmRunner;

namespace SimpleTarskiAlgorithmConsoleApp
{
    class Program
    {
        static void Main()
        {
            Console.Write("Please enter the formula: ");
            var formulaString = Console.ReadLine();

            Formula formula;
            try
            {
                formula = Parser.ToFormula(formulaString);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Input error:\n{e}");
                return;
            }

            Console.WriteLine($"Entered formula: {formula}");

            Formula result;
            try
            {
                result = SimpleTarskiAlgorithm.QuantifiersElimination(formula);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error during elimination:\n{e}");
                throw;
            }

            Console.WriteLine($"Result: {result}");
        }
    }
}