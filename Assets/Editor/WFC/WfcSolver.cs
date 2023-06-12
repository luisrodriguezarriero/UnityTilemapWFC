using Random = System.Random;
using System;
using UnityEngine;
using static WFC.Helper;

namespace WFC
{
    public class WfcSolver
    {
        private WfcModel model;
        protected bool[][] wave;
        
        double[] weightLogWeights, distribution;
        
        int[][][] compatible;
        protected int[] observed;
        
        (int, int)[] stack;
        protected int stacksize, observedSoFar;

        protected int MX, MY;
        private int[] sumsOfOnes;
        double sumOfWeights, sumOfWeightLogWeights, startingEntropy;
        protected double[] sumsOfWeights, sumsOfWeightLogWeights, entropies;

        private bool periodic;
        
        private Heuristic heuristic;
        public WfcSolver(WfcModel model, int width, int height, Heuristic heuristic, bool periodic)
        {
            MX = width;
            MY = height;
            this.model = model;
            this.heuristic = heuristic;
            InitWave();
            this.periodic = periodic;

            distribution = new double[model.nTiles];
            observed = new int[MX * MY];
            InitEntropy();
            InitStack();

            Clear();
        }

        public bool Run(int seed, int limit)
        {
            Random random = new(seed);

            for (var l = 0; l < limit || limit < 0; l++)
            {
                var node = NextUnobservedNode(random);
                if (node >= 0)
                {
                    Observe(node, random);
                    var success = Propagate();
                    if (!success) return false;
                }
                else
                {
                    for (var i = 0; i < wave.Length; i++) for (var t = 0; t < model.nTiles; t++) if (wave[i][t]) { observed[i] = t; break; }
                    return true;
                }
            }
            return true;
        }
        int NextUnobservedNode(Random random)
        {
            if (heuristic == Heuristic.Scanline) return NextUnobservedNodeScanline();
        
            var min = 1E+4;
            var argmin = -1;
        
            for (var i = 0; i < wave.Length; i++)
            {
                if (!periodic && (i % MX + model.nTiles > MX || i / MX + model.nTiles > MY)) continue;
                var remainingValues = sumsOfOnes[i];
                var entropy = heuristic == Heuristic.Entropy ? entropies[i] : remainingValues;
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
        int NextUnobservedNodeScanline(){
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
        protected void Observe(int node, Random random)
        {
            var w = wave[node];
            for (var t = 0; t < model.nTiles; t++) distribution[t] = w[t] ? model.weights[t] : 0.0;
            var r = distribution.Random(random.NextDouble());
            for (var t = 0; t < model.nTiles; t++) if (w[t] != (t == r)) Ban(node, t);
        }
        protected bool Propagate()
        {
            while (stacksize > 0)
            {
                (var i1, var t1) = stack[stacksize - 1];
                stacksize--;

                var x1 = i1 % MX;
                var y1 = i1 / MX;

                for (var d = 0; d < 4; d++)
                {
                    var x2 = x1 + dx[d];
                    var y2 = y1 + dy[d];
                    if (!periodic && (x2 < 0 || y2 < 0 || x2 + model.nTiles > MX || y2 + model.nTiles > MY)) continue;

                    if (x2 < 0) x2 += MX;
                    else if (x2 >= MX) x2 -= MX;
                    if (y2 < 0) y2 += MY;
                    else if (y2 >= MY) y2 -= MY;

                    var i2 = x2 + y2 * MX;
                    var p = model.propagator[d][t1];
                    var compat = compatible[i2];

                    for (var l = 0; l < p.Length; l++)
                    {
                        var t2 = p[l];
                        var comp = compat[t2];

                        comp[d]--;
                        if (comp[d] == 0) Ban(i2, t2);
                    }
                }
            }

            return sumsOfOnes[0] > 0;
        }
        void Ban(int i, int t)
        {
            wave[i][t] = false;

            var comp = compatible[i][t];
            for (var d = 0; d < 4; d++) comp[d] = 0;
            stack[stacksize] = (i, t);
            stacksize++;

            sumsOfOnes[i] -= 1;
            sumsOfWeights[i] -= model.weights[t];
            sumsOfWeightLogWeights[i] -= weightLogWeights[t];

            var sum = sumsOfWeights[i];
            entropies[i] = Math.Log(sum) - sumsOfWeightLogWeights[i] / sum;
        }
        void Clear()
        {
            for (var i = 0; i < wave.Length; i++)
            {
                for (var t = 0; t < model.nTiles; t++)
                {
                    wave[i][t] = true;
                    for (var d = 0; d < 4; d++) compatible[i][t][d] = model.propagator[opposite[d]][t].Length;
                }

                sumsOfOnes[i] = model.weights.Length;
                sumsOfWeights[i] = sumOfWeights;
                sumsOfWeightLogWeights[i] = sumOfWeightLogWeights;
                entropies[i] = startingEntropy;
                observed[i] = -1;
            }
            observedSoFar = 0;

            if (model.ground)
            {
                for (var x = 0; x < MX; x++)
                {
                    for (var t = 0; t < model.nTiles - 1; t++) Ban(x + (MY - 1) * MX, t);
                    for (var y = 0; y < MY - 1; y++) Ban(x + y * MX, model.nTiles - 1);
                }
                Propagate();
            }
        }
        //Initializers
        private void InitWave()
        {
            wave = new bool[MX * MY][];
            compatible = new int[wave.Length][][];
            for (var i = 0; i < wave.Length; i++)
            {
                wave[i] = new bool[model.nTiles];
                compatible[i] = new int[model.nTiles][];
                for (var t = 0; t < model.nTiles; t++) compatible[i][t] = new int[4];
            }
        }
        private void InitEntropy()
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
        private void InitStack()
        {
            stack = new (int, int)[wave.Length * model.nTiles];
            stacksize = 0;
        }
        public int[] Result => this.observed;
    }
}