using Random = System.Random;
using System;
using static WFC.Utilities;

namespace WFC.Solution
{
    public class Scanline : Solver
    {
        public Scanline(Model model, int width, int height, bool periodic) : base(model, width, height, periodic)
        {
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

        protected override int NextUnobservedNode(Random random){
            for (var i = observedSoFar; i < wave.Length; i++)
            {
                if (!periodic && (i % MX + model.nTiles > MX || i / MX + model.nTiles > MY)) continue;
                if (sumsOfOnes[i] > 1)
                {
                    observedSoFar = i + 1;
                    return i;
                }
            }
            return -1;
        }
    }
}