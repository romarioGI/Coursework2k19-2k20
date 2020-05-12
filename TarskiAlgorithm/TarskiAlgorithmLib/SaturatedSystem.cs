using System.Collections.Generic;

namespace TarskiAlgorithmLib
{
    public class SaturatedSystem
    {
        private class Node
        {
            public readonly PolynomialMonomial Polynomial;
            public readonly List<Node> Children;
            public readonly bool IsAdded;
            public readonly Hypothesis Hypothesis;

            public Node(PolynomialMonomial polynomial, Hypothesis hypothesis, Node parent, bool isAdded = false)
            {
                
            }
        }

        private readonly List<Node> _leaves;

        public SaturatedSystem()
        {
            _leaves = new List<Node> {null};
        }

        public void Add(PolynomialMonomial polynomial)
        {
            if (polynomial.IsZero)
                return;
        }
    }
}