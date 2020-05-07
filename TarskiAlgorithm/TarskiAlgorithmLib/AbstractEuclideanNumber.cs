namespace TarskiAlgorithmLib
{
    public abstract class AbstractEuclideanNumber : AbstractNumber
    {
        public static AbstractNumber GreatestCommonDivisor(AbstractEuclideanNumber first,
            AbstractEuclideanNumber second)
        {
            var firstNum = (AbstractNumber) first;
            var secondNum = (AbstractNumber) second;
            while (!secondNum.IsZero)
            {
                firstNum %= secondNum;
                var temp = firstNum;
                firstNum = secondNum;
                secondNum = temp;
            }

            return firstNum;
        }
    }
}