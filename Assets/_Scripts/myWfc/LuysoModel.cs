using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LuysoWFC
{
    abstract public class LuysoModel
    {
        protected bool[][] wave;

        protected int[][][] propagator;
        int[][][] compatible;

        (int, int)[] stack;
        int stacksize;//In position X of the wave, try Y
        HeuristicType heuristicType;

        protected int N;
        protected int MX;
        protected int MY;
        protected bool periodic;
        protected LuysoHeuristic heuristic;

        protected double[] weights, distribution;
        protected int T;

        public LuysoModel(int outputWidth, int outputHeight, int patternSize, bool periodic, HeuristicType heuristicType)
        {
            this.N = patternSize;
            this.MX = outputWidth;
            this.MY = outputHeight;
            this.periodic = periodic;
            this.heuristicType = heuristicType;
        }

        internal void CreateNewTileMap()
        {
            throw new NotImplementedException();//  TODO
        }

        internal object GetOutputTileMap()
        {
            throw new NotImplementedException();// TODO
        }

        public override String ToString()
        {
            String result = "";
            for (int y = 0; y < MY; y++)
            {
                for (int x = 0; x < MX; x++)
                {
                    result += heuristic.observed[MX * y + x] + " ";
                }
                result += "\n";
            }

            return result;
        }
        protected void Init()
        {
            InitWave();
            InitCompatible();
            distribution = new double[T];
            InitHeuristic();
            InitStack();
        }

        private void InitCompatible()
        {
            compatible = new int[wave.Length][][];
            for (int i = 0; i < wave.Length; i++)
            {
                compatible[i] = new int[T][];
                for (int t = 0; t < T; t++) compatible[i][t] = new int[4];
            }
        }
        protected abstract void InitPropagator();
        void InitHeuristic()
        {
            switch (heuristicType)
            {
                case HeuristicType.Entropy:
                    heuristic = new Entropy(wave.Length, T, weights);
                    break;
                case HeuristicType.Scanline:
                    heuristic = new Scanline(wave.Length);
                    break;
                case HeuristicType.MRV:
                    heuristic = new MRV(wave.Length);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return;
        }
        private void InitWave()
        {
            wave = new bool[MX * MY][];
            for (int i = 0; i < wave.Length; i++)
            {
                wave[i] = new bool[T];
            }
        }
        private void InitStack()
        {
            stacksize = 0;
            stack = new (int, int)[wave.Length * T];
        }
        protected abstract bool isNodeExcluded(int n);
        public bool Run(int seed, int limit)
        {
            if (wave == null) Init();

            Clear();
            System.Random random = new(seed);
            for (int l = 0; l < limit || limit < 0; l++)
            {
                int node = heuristic.NextUnobservedNode(random, isNodeExcluded);
                if (node >= 0)
                {
                    Observe(node, random);
                    bool success = Propagate();
                    if (!success) return false;

                }
                else
                {

                    for (int i = 0; i < wave.Length; i++)
                        for (int t = 0; t < T; t++) if (wave[i][t])
                            {
                                heuristic.observed[i] = t;
                                break;

                            }
                    Debug.Log(waveToString());

                    return true;
                }
            }
            Debug.Log(waveToString());
            //Debug.Log(ToString());
            return heuristic.observedSoFar > 0;
        }
        public void printCompatible()
        {
            String Result = "";
            for (int i = 0; i < compatible.Length; i++)
            {
                Result += ("Para el nodo número " + i + "/" + compatible.Length + "\n");
                for (int j = 0; j < compatible[i].Length; j++)
                {
                    Result += ("El patrón número " + j + "\n tiene estas compatibilidades en las 4 direcciones: \n");
                    for (int k = 0; k < compatible[i][j].Length; k++)
                    {
                        Result += (compatible[i][j][k] + " ");
                    }
                    Result += ("\n");
                }
                Result += ("\n\n");
            }
            //Debug.Log(Result);
        }
        private bool Propagate()
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
                    if (!periodic && (x2 < 0 || y2 < 0 || x2 + N > MX || y2 + N > MY)) continue;

                    if (x2 < 0) x2 += MX;
                    else if (x2 >= MX) x2 -= MX;
                    if (y2 < 0) y2 += MY;
                    else if (y2 >= MY) y2 -= MY;

                    int i2 = x2 + y2 * MX;
                    int[] p = propagator[d][t1]; // Patrones (t2) con los que t1 es compatible en la dirección d 
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

            return heuristic.sumsOfOnes[0] > 0;
        }
        private void Observe(int node, System.Random random)
        {
            bool[] w = wave[node];
            for (int t = 0; t < T; t++) distribution[t] = w[t] ? weights[t] : 0.0;
            int r = distribution.Random(random.NextDouble());
            for (int t = 0; t < T; t++) if (w[t] != (t == r)) Ban(node, t);
        }
        private void Ban(int i, int t)
        {
            wave[i][t] = false;
            int[] comp = compatible[i][t];

            for (int d = 0; d < 4; d++) comp[d] = 0;

            stack[stacksize] = (i, t);
            stacksize++;

            heuristic.Ban(i, t);
        }
        private void Clear()
        {
            for (int i = 0; i < wave.Length; i++)
            {
                for (int t = 0; t < T; t++)
                {
                    wave[i][t] = true;
                    for (int d = 0; d < 4; d++) compatible[i][t][d] = propagator[opposite[d]][t].Length;
                }
            }
            heuristic.Clear();
        }
        private String waveToString()
        {
            String result = "";
            for (int i = 0; i < wave.Length; i++)
            {
                for (int j = 0; j < wave[i].Length; j++)
                {
                    result += wave[i][j] + " ";
                }
                result += "\n";
            }
            return result;
        }
        public abstract void Save(string filename);
        protected static int[] dx = { -1, 0, 1, 0 };
        protected static int[] dy = { 0, 1, 0, -1 };
        static int[] opposite = { 2, 3, 0, 1 };
    }
}



