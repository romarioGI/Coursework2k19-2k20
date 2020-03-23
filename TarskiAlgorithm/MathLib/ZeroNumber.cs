namespace MathLib
{
    public sealed class ZeroNumber : INumber
    {
        private static ZeroNumber _instance;

        private ZeroNumber()
        { }

        public static ZeroNumber GetInstance() => _instance ??= new ZeroNumber();

        public Sign Sign => Sign.Zero;

        INumber INumber.AddNotZeroAndEqualTypes(INumber number)
        {
            return number;
        }

        INumber INumber.SubtractNotZeroAndEqualTypes(INumber number)
        {
            return number;
        }

        INumber INumber.MultiplyNotZeroAndEqualTypes(INumber number)
        {
            return this;
        }

        INumber INumber.DivideNotZeroAndEqualTypes(INumber number)
        {
            return this;
        }

        INumber INumber.GetRemainderNotZeroAndEqualTypes(INumber number)
        {
            return this;
        }

        public bool Equals(INumber other)
        {
            return other != null && other.Sign == Sign.Zero;
        }
    }
}