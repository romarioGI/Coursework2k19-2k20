using System;
using MathLib.Exceptions;

namespace MathLib
{
    public abstract class AbstractNumber : IEquatable<AbstractNumber>
    {
        public abstract Sign Sign { get; }

        public bool IsZero => Sign == Sign.Zero;

        public bool CanZero => Sign.HasFlag(Sign.Zero);

        public static AbstractNumber Zero = ZeroNumber.GetInstance();

        public static AbstractNumber operator +(AbstractNumber first, AbstractNumber second)
        {
            if (first is null || second is null)
                throw new ArgumentNullException();

            return first.Add(second);
        }

        private AbstractNumber Add(AbstractNumber abstractNumber)
        {
            if (abstractNumber is ZeroNumber)
                return this;
            if (this is ZeroNumber || GetType() == abstractNumber.GetType())
                return AddNotZeroAndEqualTypes(abstractNumber);
            throw new NumberTypeException(this, abstractNumber);
        }

        protected abstract AbstractNumber AddNotZeroAndEqualTypes(AbstractNumber abstractNumber);

        public static AbstractNumber operator -(AbstractNumber first, AbstractNumber second)
        {
            if (first is null || second is null)
                throw new ArgumentNullException();

            return first.Subtract(second);
        }

        private AbstractNumber Subtract(AbstractNumber abstractNumber)
        {
            if (abstractNumber is ZeroNumber)
                return this;
            if (this is ZeroNumber || GetType() == abstractNumber.GetType())
                return SubtractNotZeroAndEqualTypes(abstractNumber);
            throw new NumberTypeException(this, abstractNumber);
        }

        protected abstract AbstractNumber SubtractNotZeroAndEqualTypes(AbstractNumber abstractNumber);

        public static AbstractNumber operator -(AbstractNumber first)
        {
            if (first is null)
                throw new ArgumentNullException();
            return first.GetOpposite();
        }

        protected abstract AbstractNumber GetOpposite();
            
        public static AbstractNumber operator *(AbstractNumber first, AbstractNumber second)
        {
            if (first is null || second is null)
                throw new ArgumentNullException();

            return first.Multiply(second);
        }

        private AbstractNumber Multiply(AbstractNumber abstractNumber)
        {
            if (abstractNumber is ZeroNumber)
                return Zero;
            if (this is ZeroNumber || GetType() == abstractNumber.GetType())
                return MultiplyNotZeroAndEqualTypes(abstractNumber);
            throw new NumberTypeException(this, abstractNumber);
        }

        protected abstract AbstractNumber MultiplyNotZeroAndEqualTypes(AbstractNumber abstractNumber);

        public static AbstractNumber operator *(AbstractNumber first, int second)
        {
            if (first is null)
                throw new ArgumentNullException();
            return first.Multiply(second);
        }

        public static AbstractNumber operator *(int first, AbstractNumber second)
        {
            if (second is null)
                throw new ArgumentNullException();
            return second.Multiply(first);
        }

        private AbstractNumber Multiply(int number)
        {
            if (number < 0)
                return GetOpposite().Multiply(-number);

            var result = Zero;
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

        public static AbstractNumber operator /(AbstractNumber first, AbstractNumber second)
        {
            if (first is null || second is null)
                throw new ArgumentNullException();
            return first.Divide(second);
        }

        private AbstractNumber Divide(AbstractNumber abstractNumber)
        {
            if (abstractNumber.CanZero)
                throw new DivideByZeroException();
            if (this is ZeroNumber || GetType() == abstractNumber.GetType())
                return DivideNotZeroAndEqualTypes(abstractNumber);
            throw new NumberTypeException(this, abstractNumber);
        }

        protected abstract AbstractNumber DivideNotZeroAndEqualTypes(AbstractNumber abstractNumber);

        public static AbstractNumber operator %(AbstractNumber first, AbstractNumber second)
        {
            if (first is null || second is null)
                throw new ArgumentNullException();
            return first.GetRemainder(second);
        }

        private AbstractNumber GetRemainder(AbstractNumber abstractNumber)
        {
            if (abstractNumber.CanZero)
                throw new DivideByZeroException();
            if (this is ZeroNumber || GetType() == abstractNumber.GetType())
                return GetRemainderNotZeroAndEqualTypes(abstractNumber);
            throw new NumberTypeException(this, abstractNumber);
        }

        protected abstract AbstractNumber GetRemainderNotZeroAndEqualTypes(AbstractNumber abstractNumber);

        public bool Equals(AbstractNumber abstractNumber)
        {
            if (abstractNumber is null)
                return false;
            if (abstractNumber.IsZero)
                return IsZero;
            if (IsZero)
                return abstractNumber.IsZero;

            return GetType() == abstractNumber.GetType() && EqualsNotZeroAndEqualType(abstractNumber);
        }

        protected abstract bool EqualsNotZeroAndEqualType(AbstractNumber other);

        public sealed override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is AbstractNumber numberAbstract && Equals(numberAbstract);
        }

        public abstract override int GetHashCode();
    }
}