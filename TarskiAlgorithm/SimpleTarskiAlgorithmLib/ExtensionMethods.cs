using System;

namespace SimpleTarskiAlgorithmLib
{
    public static class ExtensionMethods
    {
        public static Sign Invert(this Sign sign)
        {
            return sign switch
            {
                Sign.NotNumber => Sign.NotNumber,
                Sign.LessZero => Sign.MoreZero,
                Sign.MoreZero => Sign.LessZero,
                Sign.Zero => Sign.Zero,
                Sign.NotLessZero => Sign.NotMoreZero,
                Sign.NotMoreZero => Sign.NotLessZero,
                Sign.NotZero => Sign.NotZero,
                Sign.Undefined => Sign.Undefined,
                _ => throw new NotImplementedException()
            };
        }
    }
}