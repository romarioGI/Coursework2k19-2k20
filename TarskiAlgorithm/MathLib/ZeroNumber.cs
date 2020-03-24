namespace MathLib
{
    public sealed class ZeroNumber : AbstractNumber
    {
        private static ZeroNumber _instance;

        private ZeroNumber()
        {
        }

        internal static ZeroNumber GetInstance() => _instance ??= new ZeroNumber();

        public override Sign Sign => Sign.Zero;

        protected override AbstractNumber AddNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return abstractNumber;
        }

        protected override AbstractNumber SubtractNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return -abstractNumber;
        }

        protected override AbstractNumber GetOpposite()
        {
            return this;
        }

        protected override AbstractNumber MultiplyNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return this;
        }

        protected override AbstractNumber DivideNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return this;
        }

        protected override AbstractNumber GetRemainderNotZeroAndEqualTypes(AbstractNumber abstractNumber)
        {
            return this;
        }

        protected override bool EqualsNotZeroAndEqualType(AbstractNumber other)
        {
            return other.IsZero;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}