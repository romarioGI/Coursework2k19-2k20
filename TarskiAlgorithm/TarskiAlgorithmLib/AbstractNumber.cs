using System;
using TarskiAlgorithmLib.Exceptions;

namespace TarskiAlgorithmLib
{
    public abstract class AbstractNumber : AbstractSigned, IEquatable<AbstractNumber>
    {
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

            var sum = first.AddNotZeroAndSameTypes(second);
            var sumSign = first.Sign.Add(second.Sign);
            sum.Sign &= sumSign;

            return sum;
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

            var sub = first.SubtractNotZeroAndSameTypes(second);
            var subSign = first.Sign.Subtract(second.Sign);
            sub.Sign &= subSign;

            return sub;
        }

        protected abstract AbstractNumber SubtractNotZeroAndSameTypes(AbstractNumber abstractNumber);

        public static AbstractNumber operator -(AbstractNumber first)
        {
            if (first is null)
                throw new ArgumentNullException();

            if (first.IsZero)
                return first;

            var opp = first.GetOppositeNotZero();
            var oppSign = first.Sign.Invert();
            opp.Sign &= oppSign;

            return opp;
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

            var mul = first.MultiplyNotZeroAndSameTypes(second);
            var mulSign = first.Sign.Multi(second.Sign);
            mul.Sign &= mulSign;

            return mul;
        }

        protected abstract AbstractNumber MultiplyNotZeroAndSameTypes(AbstractNumber abstractNumber);

        public static AbstractNumber operator *(AbstractNumber first, int second)
        {
            if (first is null)
                throw new ArgumentNullException();

            if (first.IsZero)
                return first;

            var mul = first.Multiply(second);
            var mulSign = first.Sign.Multi(second.GetSign());
            mul.Sign &= mulSign;

            return mul;
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

            if (second.Can(Sign.Zero))
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

            if (second.Can(Sign.Zero))
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
    }
}