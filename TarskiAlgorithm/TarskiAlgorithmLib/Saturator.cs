using System.Collections.Generic;

namespace TarskiAlgorithmLib
{
    public static class Saturator
    {
        public static SaturatedSystem Saturate(IEnumerable<PolynomialMonomial> polynomials)
        {
            var result = new SaturatedSystem();
            foreach (var p in polynomials)
                result.Add(p);

            return result;
        }
    }
}