using Random = System.Random;
using static WFC.Utilities;
using System.Collections.Generic;

namespace WFC.Solution
{
    public abstract class Solver
    {
        protected Model model;

        protected int MX, MY;
        protected bool periodic;

        protected bool[][] wave;
        protected int[][][] compatible;

        public int[] observed {get; protected set;}
        protected int observedSoFar;
        protected double[] distribution;  
        protected int[] sumsOfOnes;

        protected Stack<(int, int)> stack;

        public Solver(Model model, int width, int height, bool periodic)
        {
            MX = width;
            MY = height;
            this.model = model;
            this.periodic = periodic;

            distribution = new double[model.nTiles];
            observed = new int[MX * MY];

            InitWave();
            InitEntropy();
            InitStack();

            Clear();

            if (model.ground) 
                ClearGround();
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

        public bool RunOneStep(int seed)
        {
            Random random = new(seed);

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
            return true;
        }
        protected abstract int NextUnobservedNode(Random random);

        protected void Observe(int node, Random random)
        {
            var w = wave[node];
            for (var t = 0; t < model.nTiles; t++) distribution[t] = w[t] ? model.weights[t] : 0.0;
            var r = distribution.Random(random.NextDouble());
            for (var t = 0; t < model.nTiles; t++) if (w[t] != (t == r)) Ban(node, t);
        }

        protected bool Propagate()
        {
            while (stack.Count > 0)
            {
                (var i1, var t1) = stack.Pop();

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
        protected virtual void Ban(int i, int t)
        {
            wave[i][t] = false;

            var comp = compatible[i][t];
            for (var d = 0; d < 4; d++) comp[d] = 0;

            stack.Push(i, t);

            sumsOfOnes[i] -= 1;
        }
        protected virtual void Clear()
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

        protected void ClearGround(){
            for (var x = 0; x < MX; x++)
                {
                    for (var t = 0; t < model.nTiles - 1; t++) Ban(x + (MY - 1) * MX, t);
                    for (var y = 0; y < MY - 1; y++) Ban(x + y * MX, model.nTiles - 1);
                }
                Propagate();
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

        protected virtual void InitEntropy()
        {
            sumsOfOnes = new int[MX * MY];
        }
        private void InitStack()
        {
            stack = new Stack<(int, int)>();
        }
        public int[] Result => this.observed;


    }
}