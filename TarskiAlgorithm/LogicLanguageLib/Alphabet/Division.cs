namespace LogicLanguageLib.Alphabet
{
    public sealed class Division : ArithmeticBinaryFunction
    {
        private static readonly Division Instance = new Division("/");

        private Division(string name) : base(name)
        {
        }

        public static Division GetInstance()
        {
            return Instance;
        }

        public override int Priority => 120;
    }
}