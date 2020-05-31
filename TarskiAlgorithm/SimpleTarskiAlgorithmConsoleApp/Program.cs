using System;
using System.IO;
using LogicLanguageLib.IO;
using LogicLanguageLib.Words;
using SimpleTarskiAlgorithmRunner;

namespace SimpleTarskiAlgorithmConsoleApp
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Tarski";
            using (var sw = new StreamWriter("OUTPUT1.txt"))
            {
                Console.Write("Please enter the formula: ");
                var formulaString = Console.ReadLine();
                Console.WriteLine("waiting");
                Formula formula;
                try
                {
                    formula = Parser.ToFormula(formulaString);
                }
                catch (Exception e)
                {
                    sw.WriteLine($"Input error:\n{e}");
                    return;
                }

                sw.WriteLine(formulaString);
                sw.WriteLine($"Entered formula: {formula}");

                Formula result;
                try
                {
                    result = SimpleTarskiAlgorithm.QuantifiersElimination(formula);
                }
                catch (Exception e)
                {
                    sw.WriteLine($"Error during elimination:\n{e}");
                    throw;
                }

                sw.WriteLine($"Result: {result}");
                sw.Close();
                Console.WriteLine("see results in OUTPUT.txt");
            }
        }
    }
}