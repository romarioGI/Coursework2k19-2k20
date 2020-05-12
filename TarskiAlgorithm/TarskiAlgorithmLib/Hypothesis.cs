using SimpleTarskiAlgorithmLib;

namespace TarskiAlgorithmLib
{
    public class Hypothesis
    {
        public readonly RationalMonomialsNumber Number;
        public readonly Sign Sign;

        public Hypothesis(RationalMonomialsNumber number, Sign sign)
        {
            Number = number;
            Sign = sign;
        }
    }
}