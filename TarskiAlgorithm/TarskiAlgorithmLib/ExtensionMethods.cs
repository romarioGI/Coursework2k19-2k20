using System;
using SimpleTarskiAlgorithmLib;

namespace TarskiAlgorithmLib
{
    //TODO перепроверить
    public static class ExtensionMethods
    {
        public static Sign Add(this Sign first, Sign second)
        {
            return first switch
            {
                Sign.NotNumber => Sign.NotNumber,
                Sign.LessZero => second switch
                {
                    Sign.NotNumber => Sign.NotNumber,
                    Sign.LessZero => Sign.LessZero,
                    Sign.Zero => Sign.LessZero,
                    Sign.NotMoreZero => Sign.LessZero,
                    _ => Sign.Undefined
                },
                Sign.MoreZero => second switch
                {
                    Sign.NotNumber => Sign.NotNumber,
                    Sign.MoreZero => Sign.MoreZero,
                    Sign.Zero => Sign.MoreZero,
                    Sign.NotLessZero => Sign.MoreZero,
                    _ => Sign.Undefined
                },
                Sign.Zero => second,
                Sign.NotMoreZero => second switch
                {
                    Sign.NotNumber => Sign.NotNumber,
                    Sign.LessZero => Sign.LessZero,
                    Sign.Zero => Sign.NotMoreZero,
                    Sign.NotMoreZero => Sign.NotMoreZero,
                    _ => Sign.Undefined
                },
                Sign.NotLessZero => second switch
                {
                    Sign.NotNumber => Sign.NotNumber,
                    Sign.MoreZero => Sign.MoreZero,
                    Sign.Zero => Sign.NotLessZero,
                    Sign.NotLessZero => Sign.NotLessZero,
                    _ => Sign.Undefined
                },
                Sign.NotZero => second switch
                {
                    Sign.NotNumber => Sign.NotNumber,
                    Sign.Zero => Sign.NotZero,
                    _ => Sign.Undefined
                },
                Sign.Undefined => second switch
                {
                    Sign.NotNumber => Sign.NotNumber,
                    _ => Sign.Undefined
                },
                _ => throw new NotSupportedException()
            };
        }

        public static Sign Subtract(this Sign first, Sign second)
        {
            return first.Add(second.Invert());
        }

        public static Sign Multi(this Sign first, Sign second)
        {
            return first switch
            {
                Sign.NotNumber => Sign.NotNumber,
                Sign.LessZero => second switch
                {
                    Sign.NotNumber => Sign.NotNumber,
                    Sign.LessZero => Sign.MoreZero,
                    Sign.MoreZero => Sign.LessZero,
                    Sign.Zero => Sign.Zero,
                    Sign.NotMoreZero => Sign.NotLessZero,
                    Sign.NotLessZero => Sign.NotMoreZero,
                    Sign.NotZero => Sign.NotZero,
                    _ => Sign.Undefined
                },
                Sign.MoreZero => second switch
                {
                    Sign.NotNumber => Sign.NotNumber,
                    Sign.LessZero => Sign.LessZero,
                    Sign.MoreZero => Sign.MoreZero,
                    Sign.Zero => Sign.Zero,
                    Sign.NotMoreZero => Sign.NotMoreZero,
                    Sign.NotLessZero => Sign.NotLessZero,
                    Sign.NotZero => Sign.NotZero,
                    _ => Sign.Undefined
                },
                Sign.Zero => second switch
                {
                    Sign.NotNumber => Sign.NotNumber,
                    _ => Sign.Zero
                },
                Sign.NotMoreZero => second switch
                {
                    Sign.NotNumber => Sign.NotNumber,
                    Sign.LessZero => Sign.NotLessZero,
                    Sign.MoreZero => Sign.NotMoreZero,
                    Sign.Zero => Sign.Zero,
                    Sign.NotMoreZero => Sign.NotLessZero,
                    Sign.NotLessZero => Sign.NotMoreZero,
                    _ => Sign.Undefined
                },
                Sign.NotLessZero => second switch
                {
                    Sign.NotNumber => Sign.NotNumber,
                    Sign.LessZero => Sign.NotMoreZero,
                    Sign.MoreZero => Sign.NotLessZero,
                    Sign.Zero => Sign.Zero,
                    Sign.NotMoreZero => Sign.NotMoreZero,
                    Sign.NotLessZero => Sign.NotLessZero,
                    _ => Sign.Undefined
                },
                Sign.NotZero => second switch
                {
                    Sign.NotNumber => Sign.NotNumber,
                    Sign.LessZero => Sign.NotZero,
                    Sign.MoreZero => Sign.NotZero,
                    Sign.Zero => Sign.Zero,
                    Sign.NotZero => Sign.NotZero,
                    _ => Sign.Undefined
                },
                Sign.Undefined => second switch
                {
                    Sign.NotNumber => Sign.NotNumber,
                    Sign.Zero => Sign.Zero,
                    _ => Sign.Undefined
                },
                _ => throw new NotSupportedException()
            };
        }

        public static Sign Divide(this Sign first, Sign second)
        {
            if (second.HasFlag(Sign.Zero))
                throw new DivideByZeroException();

            return first.Multi(second);
        }

        public static Sign GetSign(this int number)
        {
            if (number < 0)
                return Sign.LessZero;
            if (number == 0)
                return Sign.Zero;
            return Sign.MoreZero;
        }

        public static bool IsBasic(this Sign sign)
        {
            return sign == Sign.Zero || sign == Sign.MoreZero || sign == Sign.LessZero;
        }
    }
}