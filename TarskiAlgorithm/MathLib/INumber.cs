using System;

namespace MathLib
{
    public interface INumber : IEquatable<INumber>
    {
        public Sign Sign { get; }

        public bool IsZero => Sign == Sign.Zero;

        public bool CanZero => Sign.HasFlag(Sign.Zero);

        public static INumber Zero = ZeroNumber.GetInstance();

        public INumber Add(INumber number)
        {
            if (number is ZeroNumber)
                return this;
            if (this is ZeroNumber || GetType() == number.GetType())
                return AddNotZeroAndEqualTypes(number);
            throw new NumberTypeException(this, number);
        }

        protected INumber AddNotZeroAndEqualTypes(INumber number);

        public INumber Subtract(INumber number)
        {
            if (number is ZeroNumber)
                return this;
            if (this is ZeroNumber || GetType() == number.GetType())
                return SubtractNotZeroAndEqualTypes(number);
            throw new NumberTypeException(this, number);
        }

        protected INumber SubtractNotZeroAndEqualTypes(INumber number);

        public INumber Multiply(INumber number)
        {
            if (number is ZeroNumber)
                return number;
            if (this is ZeroNumber || GetType() == number.GetType())
                return MultiplyNotZeroAndEqualTypes(number);
            throw new NumberTypeException(this, number);
        }

        protected INumber MultiplyNotZeroAndEqualTypes(INumber number);

        public INumber Multiply(int number)
        {
            if (number < 0)
                return Subtract(this).Subtract(this).Multiply(-number);

            var result = Subtract(this); // this - this = 0
            var current = this;
            while (number != 0)
            {
                if ((number & 1) == 1)
                    result = result.Add(current);

                number >>= 1;
                current = current.Add(current);
            }

            return result;
        }

        public INumber Divide(INumber number)
        {
            if (number.CanZero)
                throw new DivideByZeroException();
            if (this is ZeroNumber || GetType() == number.GetType())
                return DivideNotZeroAndEqualTypes(number);
            throw new NumberTypeException(this, number);
        }

        protected INumber DivideNotZeroAndEqualTypes(INumber number);

        public INumber GetRemainder(INumber number)
        {
            if (number.CanZero)
                throw new DivideByZeroException();
            if (this is ZeroNumber || GetType() == number.GetType())
                return GetRemainderNotZeroAndEqualTypes(number);
            throw new NumberTypeException(this, number);
        }

        protected INumber GetRemainderNotZeroAndEqualTypes(INumber number);
    }
}