using System;

namespace MathLib
{
    public interface INumber : IEquatable<INumber>
    {
        Sign Sign { get; }

        public bool IsZero => Sign == Sign.Zero;

        INumber Add(INumber number);

        INumber Subtract(INumber number);

        INumber Multiply(INumber number);

        INumber Divide(INumber number);

        INumber GetRemainder(INumber number);
    }
}