using System;
using System.Linq;

namespace LogicLanguageLib
{
    public class ObjectVariable : LogicalSymbol
    {
        public ObjectVariable(string name, string subscript) : base(Concatenate(name, subscript))
        {
        }

        private static string Concatenate(string name, string subscript)
        {
            if (!CheckName(name))
                throw new ArgumentException("Name must contain only letters");

            if (string.IsNullOrEmpty(subscript))
                return name;

            if (!CheckSubscript(subscript))
                throw new ArgumentException("Name must contain only digits");

            return $"{name}_{{{subscript}}}";
        }

        private static bool CheckName(string name)
        {
            return name.All(char.IsLetter);
        }

        private static bool CheckSubscript(string subscript)
        {
            return subscript.All(char.IsDigit);
        }
    }
}