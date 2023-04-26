using System.Collections.Generic;
using System.Collections;
using Random = System.Random;
using UnityEngine;
using System;

namespace WFC_Unity_Luyso
{
    public class WfcSolver
    {
        private TilemapWfcModel model;
        protected bool[][] wave;
        
        double[] weightLogWeights, distribution;
        
        int[][][] compatible;
        protected int[] observed;
        
        (int, int)[] stack;
        protected int stacksize, observedSoFar;

        protected int MX, MY;
        protected int[] sumsOfOnes;
        double sumOfWeights, sumOfWeightLogWeights, startingEntropy;
        protected double[] sumsOfWeights, sumsOfWeightLogWeights, entropies;

        public GameObject outputGrid;
        
        private Heuristic heuristic;
        public WfcSolver(TilemapWfcModel model, int width, int height)
        {
            MX = width;
            MY = height;
            this.model = model;
            initWave();

            distribution = new double[model.nTiles];
            observed = new int[MX * MY];
            initEntropy();
            initStack();

            Clear();
        }

        public bool Run(int seed, int limit)
        {
            Random random = new(seed);

            for (int l = 0; l < limit || limit < 0; l++)
            {
                int node = NextUnobservedNode(random);
                if (node >= 0)
                {
                    Observe(node, random);
                    bool success = Propagate();
                    if (!success) return false;
                }
                else
                {
                    for (int i = 0; i < wave.Length; i++) for (int t = 0; t < model.nTiles; t++) if (wave[i][t]) { observed[i] = t; break; }
                    return true;
                }
            }
            return true;
        }

        int NextUnobservedNode(Random random)
        {
            if (heuristic == Heuristic.Scanline) return NextUnobservedNodeScanline();
        
            double min = 1E+4;
            int argmin = -1;
        
            for (int i = 0; i < wave.Length; i++)
            {
                if (!model.Periodic && (i % MX + model.nTiles > MX || i / MX + model.nTiles > MY)) continue;
                int remainingValues = sumsOfOnes[i];
                double entropy = heuristic == Heuristic.Entropy ? entropies[i] : remainingValues;
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

        int NextUnobservedNodeScanline(){
            for (int i = observedSoFar; i < wave.Length; i++)
            {
                if (!model.Periodic && (i % MX + model.nTiles > MX || i / MX + model.nTiles > MY)) continue;
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
            bool[] w = wave[node];
            for (int t = 0; t < model.nTiles; t++) distribution[t] = w[t] ? model.Weights[t] : 0.0;
            int r = distribution.Random(random.NextDouble());
            for (int t = 0; t < model.nTiles; t++) if (w[t] != (t == r)) Ban(node, t);
        }

        protected bool Propagate()
        {
            while (stacksize > 0)
            {
                (int i1, int t1) = stack[stacksize - 1];
                stacksize--;

                int x1 = i1 % MX;
                int y1 = i1 / MX;

                for (int d = 0; d < 4; d++)
                {
                    int x2 = x1 + dx[d];
                    int y2 = y1 + dy[d];
                    if (!model.Periodic && (x2 < 0 || y2 < 0 || x2 + model.nTiles > MX || y2 + model.nTiles > MY)) continue;

                    if (x2 < 0) x2 += MX;
                    else if (x2 >= MX) x2 -= MX;
                    if (y2 < 0) y2 += MY;
                    else if (y2 >= MY) y2 -= MY;

                    int i2 = x2 + y2 * MX;
                    int[] p = model.Propagator[d][t1];
                    int[][] compat = compatible[i2];

                    for (int l = 0; l < p.Length; l++)
                    {
                        int t2 = p[l];
                        int[] comp = compat[t2];

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

            int[] comp = compatible[i][t];
            for (int d = 0; d < 4; d++) comp[d] = 0;
            stack[stacksize] = (i, t);
            stacksize++;

            sumsOfOnes[i] -= 1;
            sumsOfWeights[i] -= model.Weights[t];
            sumsOfWeightLogWeights[i] -= weightLogWeights[t];

            double sum = sumsOfWeights[i];
            entropies[i] = Math.Log(sum) - sumsOfWeightLogWeights[i] / sum;
        }

        void Clear()
        {
            for (int i = 0; i < wave.Length; i++)
            {
                for (int t = 0; t < model.nTiles; t++)
                {
                    wave[i][t] = true;
                    for (int d = 0; d < 4; d++) compatible[i][t][d] = model.Propagator[opposite[d]][t].Length;
                }

                sumsOfOnes[i] = model.Weights.Length;
                sumsOfWeights[i] = sumOfWeights;
                sumsOfWeightLogWeights[i] = sumOfWeightLogWeights;
                entropies[i] = startingEntropy;
                observed[i] = -1;
            }
            observedSoFar = 0;

            if (model.Ground)
            {
                for (int x = 0; x < MX; x++)
                {
                    for (int t = 0; t < model.nTiles - 1; t++) Ban(x + (MY - 1) * MX, t);
                    for (int y = 0; y < MY - 1; y++) Ban(x + y * MX, model.nTiles - 1);
                }
                Propagate();
            }
        }
        
        //Initializers
        private void initWave()
        {
            wave = new bool[MX * MY][];
            compatible = new int[wave.Length][][];
            for (int i = 0; i < wave.Length; i++)
            {
                wave[i] = new bool[model.nTiles];
                compatible[i] = new int[model.nTiles][];
                for (int t = 0; t < model.nTiles; t++) compatible[i][t] = new int[4];
            }
        }

        private void initEntropy()
        {
            weightLogWeights = new double[model.nTiles];
            sumOfWeights = 0;
            sumOfWeightLogWeights = 0;

            for (int t = 0; t < model.nTiles; t++)
            {
                weightLogWeights[t] = model.Weights[t] * Math.Log(model.Weights[t]);
                sumOfWeights += model.Weights[t];
                sumOfWeightLogWeights += weightLogWeights[t];
            }

            startingEntropy = Math.Log(sumOfWeights) - sumOfWeightLogWeights / sumOfWeights;

            sumsOfOnes = new int[MX * MY];
            sumsOfWeights = new double[MX * MY];
            sumsOfWeightLogWeights = new double[MX * MY];
            entropies = new double[MX * MY];
        }

        private void initStack()
        {
            stack = new (int, int)[wave.Length * model.nTiles];
            stacksize = 0;
        }

        public int[] result => this.observed;
    }
}