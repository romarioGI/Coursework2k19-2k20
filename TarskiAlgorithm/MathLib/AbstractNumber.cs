using System;
using MathLib.Exceptions;

namespace MathLib
{
    public abstract class AbstractNumber : IEquatable<AbstractNumber>
    {
        public Sign Sign { get; protected set; }

        public bool IsZero => Sign == Sign.Zero;

        public bool CanZero => Sign.HasFlag(Sign.Zero);

        public static AbstractNumber operator +(AbstractNumber first, AbstractNumber second)
        {
            if (first is null || second is null)
                throw new ArgumentNullException();

            if (first.GetType() != second.GetType())
                throw new NumberTypeException(first, second);

            if (first.IsZero)
                return second;

            if (second.IsZero)
                return first;

            return first.AddNotZeroAndSameTypes(second);
        }

        protected abstract AbstractNumber AddNotZeroAndSameTypes(AbstractNumber abstractNumber);

        public static AbstractNumber operator -(AbstractNumber first, AbstractNumber second)
        {
            if (first is null || second is null)
                throw new ArgumentNullException();

            if (first.GetType() != second.GetType())
                throw new NumberTypeException(first, second);

            if (first.IsZero)
                return -second;

            if (second.IsZero)
                return first;

            return first.SubtractNotZeroAndSameTypes(second);
        }

        protected abstract AbstractNumber SubtractNotZeroAndSameTypes(AbstractNumber abstractNumber);

        public static AbstractNumber operator -(AbstractNumber first)
        {
            if (first is null)
                throw new ArgumentNullException();

            if (first.IsZero)
                return first;

            return first.GetOppositeNotZero();
        }

        protected abstract AbstractNumber GetOppositeNotZero();

        public static AbstractNumber operator *(AbstractNumber first, AbstractNumber second)
        {
            if (first is null || second is null)
                throw new ArgumentNullException();

            if (first.GetType() != second.GetType())
                throw new NumberTypeException(first, second);

            if (first.IsZero)
                return first;

            if (second.IsZero)
                return second;

            return first.MultiplyNotZeroAndSameTypes(second);
        }

        protected abstract AbstractNumber MultiplyNotZeroAndSameTypes(AbstractNumber abstractNumber);

        public static AbstractNumber operator *(AbstractNumber first, int second)
        {
            if (first is null)
                throw new ArgumentNullException();

            if (first.IsZero)
                return first;

            return first.Multiply(second);
        }

        public static AbstractNumber operator *(int first, AbstractNumber second)
        {
            return second * first;
        }

        protected abstract AbstractNumber Multiply(int number);

        public static AbstractNumber operator /(AbstractNumber first, AbstractNumber second)
        {
            if (first is null || second is null)
                throw new ArgumentNullException();

            if (first.GetType() != second.GetType())
                throw new NumberTypeException(first, second);

            if (second.CanZero)
                throw new DivideByZeroException();

            if (first.IsZero)
                return first;

            return first.DivideNotZeroAndSameTypes(second);
        }

        protected abstract AbstractNumber DivideNotZeroAndSameTypes(AbstractNumber abstractNumber);

        public static AbstractNumber operator %(AbstractNumber first, AbstractNumber second)
        {
            if (first is null || second is null)
                throw new ArgumentNullException();

            if (first.GetType() != second.GetType())
                throw new NumberTypeException(first, second);

            if (second.CanZero)
                throw new DivideByZeroException();

            if (first.IsZero)
                return first;

            return first.GetRemainderNotZeroAndSameTypes(second);
        }

        protected abstract AbstractNumber GetRemainderNotZeroAndSameTypes(AbstractNumber abstractNumber);

        public bool Equals(AbstractNumber abstractNumber)
        {
            if (abstractNumber is null)
                return false;
            if (abstractNumber.IsZero)
                return IsZero;
            if (IsZero)
                return abstractNumber.IsZero;

            return GetType() == abstractNumber.GetType() && EqualsNotZeroAndSameType(abstractNumber);
        }

        protected abstract bool EqualsNotZeroAndSameType(AbstractNumber other);

        public sealed override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is AbstractNumber numberAbstract && Equals(numberAbstract);
        }

        public abstract override int GetHashCode();

        public AbstractNumber SetSign(Sign sign)
        {
            if (Sign.HasFlag(sign))
                return SetVerifiedSign(sign);
            throw new ArgumentException();
        }

        protected abstract AbstractNumber SetVerifiedSign(Sign sign);
    }
}