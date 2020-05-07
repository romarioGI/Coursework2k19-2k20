namespace TarskiAlgorithmLib
{
    public abstract class AbstractSigned
    {
        public Sign Sign { get; protected set; }

        public virtual bool IsZero => Sign == Sign.Zero;

        public virtual bool Can(Sign sign)
        {
            return Sign.HasFlag(sign);
        }
    }
}