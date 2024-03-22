using Random = System.Random;
using System;

namespace WFC
{
    public class EntropySolver : WfcSolver
    {
        double[] weightLogWeights;
        double sumOfWeights, sumOfWeightLogWeights, startingEntropy;
        double[] sumsOfWeights, sumsOfWeightLogWeights, entropies;
        public EntropySolver(WfcModel model, int width, int height, bool periodic) : base(model, width, height, periodic)
        {
        }

        protected override void InitEntropy()
        {
            weightLogWeights = new double[model.nTiles];
            sumOfWeights = 0;
            sumOfWeightLogWeights = 0;

            for (var t = 0; t < model.nTiles; t++)
            {
                weightLogWeights[t] = model.weights[t] * Math.Log(model.weights[t]);
                sumOfWeights += model.weights[t];
                sumOfWeightLogWeights += weightLogWeights[t];
            }

            startingEntropy = Math.Log(sumOfWeights) - sumOfWeightLogWeights / sumOfWeights;

            sumsOfOnes = new int[MX * MY];
            sumsOfWeights = new double[MX * MY];
            sumsOfWeightLogWeights = new double[MX * MY];
            entropies = new double[MX * MY];
        }

        protected override void Clear() 
        {
            base.Clear();

            for( var i = 0; i< wave.Length; i++)
            {
                sumsOfWeights[i] = sumOfWeights;
                sumsOfWeightLogWeights[i] = sumOfWeightLogWeights;
                entropies[i] = startingEntropy;
            }
        }

        protected override void Ban(int i, int t)
        {
            base.Ban(i, t);

            sumsOfWeights[i] -= model.weights[t];
            sumsOfWeightLogWeights[i] -= weightLogWeights[t];

            var sum = sumsOfWeights[i];
            entropies[i] = Math.Log(sum) - sumsOfWeightLogWeights[i] / sum;
        }

        protected override int NextUnobservedNode(Random random)
        {
            var min = 1E+4;
            var argmin = -1;
        
            for (var i = 0; i < wave.Length; i++)
            {
                if (!periodic && (i % MX + model.nTiles > MX || i / MX + model.nTiles > MY)) continue;
                var remainingValues = sumsOfOnes[i];
                var entropy = entropies[i];
                if (remainingValues > 1 && entropy <= min)
                {
                    var noise = 1E-6 * random.NextDouble();
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