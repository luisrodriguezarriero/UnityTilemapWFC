using System;
namespace LuysoWFC
{
    public class Entropy : LuysoHeuristic
    {
        protected double[] weights;
        double[] weightLogWeights;

        double sumOfWeights, sumOfWeightLogWeights, startingEntropy;
        protected double[] sumsOfWeights, sumsOfWeightLogWeights, entropies;

        public Entropy(int nNodes, int nPatterns, double[] weights) : base(nNodes)
        {
            this.weights = weights;

            weightLogWeights = new double[nPatterns];
            sumOfWeights = 0;
            sumOfWeightLogWeights = 0;
            for (int t = 0; t < nPatterns; t++)
            {
                weightLogWeights[t] = weights[t] * Math.Log(weights[t]);
                sumOfWeights += weights[t];
                sumOfWeightLogWeights += weightLogWeights[t];
            }

            startingEntropy = Math.Log(sumOfWeights) - sumOfWeightLogWeights / sumOfWeights;

            sumsOfWeights = new double[nNodes];
            sumsOfWeightLogWeights = new double[nNodes];
            entropies = new double[nNodes];
        }

        public override void Clear()
        {
            for (int i = 0; i < nNodes; i++)
            {
                sumsOfOnes[i] = nNodes;
                sumsOfWeights[i] = sumOfWeights;
                sumsOfWeightLogWeights[i] = sumOfWeightLogWeights;
                entropies[i] = startingEntropy;
                observed[i] = -1;
            }
            observedSoFar = 0;
        }
        public override void Ban(int i, int t)
        {
            sumsOfOnes[i] -= 1;
            sumsOfWeights[i] -= weights[t];
            sumsOfWeightLogWeights[i] -= weightLogWeights[t];

            double sum = sumsOfWeights[i];
            entropies[i] = Math.Log(sum) - sumsOfWeightLogWeights[i] / sum;
        }
        public double getEntropy(int i)
        {
            return entropies[i];
        }

        public override int NextUnobservedNode(Random random, Func<int, bool> isNodeExcluded)
        {
            double min = 1E+4;
            int argmin = -1;
            double entropy;

            for (int i = 0; i < nNodes; i++)
            {
                if (isNodeExcluded(i)) continue;

                int remainingValues = sumsOfOnes[i];
                entropy = entropies[i];

                if (remainingValues > 1 && entropy <= min)
                {

                    double noise = 1E-6 * random.NextDouble();
                    if (entropy + noise < min)
                    {
                        min = entropy + noise;
                        argmin = i;
                    }
                }
            }
            return argmin;
        }
    }
}
