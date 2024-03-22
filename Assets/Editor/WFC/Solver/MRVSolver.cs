using Random = System.Random;
using System;
using static WFC.WfcUtilities;

namespace WFC
{
    public class MRVSolver : WfcSolver
    {
        public MRVSolver(WfcModel model, int width, int height, bool periodic) : base(model, width, height, periodic)
        {}

        protected override int NextUnobservedNode(Random random)
        {        
            var min = 1E+4;
            var argmin = -1;
        
            for (var i = 0; i < wave.Length; i++)
            {
                if (!periodic && (i % MX + model.nTiles > MX || i / MX + model.nTiles > MY)) continue;
                var remainingValues = sumsOfOnes[i];
                if (remainingValues > 1 && remainingValues <= min)
                {
                    var noisedValue = remainingValues + 1E-6 * random.NextDouble();
                    if (noisedValue < min)
                    {
                        min = noisedValue;
                        argmin = i;
                    }
                }
            }
            return argmin;
        }

        protected override void Clear()
        {
            for (var i = 0; i < wave.Length; i++)
            {
                for (var t = 0; t < model.nTiles; t++)
                {
                    wave[i][t] = true;
                    for (var d = 0; d < 4; d++) compatible[i][t][d] = model.propagator[opposite[d]][t].Length;
                }

                sumsOfOnes[i] = model.weights.Length;
                observed[i] = -1;
            }
            observedSoFar = 0;

        }
    }
}