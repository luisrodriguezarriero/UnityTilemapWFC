using System.Collections.Generic;
using System;

namespace WFC.Tilemap.Overlap
{
    [Serializable]
    public class PatternList
    {
        public readonly List<byte[]> patterns;
        public readonly int T;
        public readonly double[] weights;
        public PatternList(byte[] sample, int sx, int sy, int n, int symmetry, bool periodicInput, int tilesCount)
        {
            var xMax = periodicInput ? sx : sx - n + 1;
            var yMax = periodicInput ? sy : sy - n + 1;
            Dictionary<long, int> patternIndices = new();

            patterns = new();

            List<double> weightList = new();

            for (var y = 0; y < yMax; y++)
                for (var x = 0; x < xMax; x++)
                {
                    var ps = new byte[8][];
                    ps[0] = pattern((dx, dy) => sample[(x + dx) % sx + (y + dy) % sy * sx], n);
                    ps[1] = reflect(ps[0], n);
                    ps[2] = rotate(ps[0], n);
                    ps[3] = reflect(ps[2], n);
                    ps[4] = rotate(ps[2], n);
                    ps[5] = reflect(ps[4], n);
                    ps[6] = rotate(ps[4], n);
                    ps[7] = reflect(ps[6], n);

                    for (var k = 0; k < symmetry; k++)
                    {
                        var p = ps[k];
                        var h = Hash(p, tilesCount);
                        if (patternIndices.TryGetValue(h, out var index)) weightList[index] = weightList[index] + 1;
                        else
                        {
                            patternIndices.Add(h, weightList.Count);
                            weightList.Add(1.0);
                            patterns.Add(p);
                        }
                    }
                }
            weights = weightList.ToArray();
            T = weights.Length; //Número de Patrones
        }
        static long Hash(byte[] p, int C)
        {
            long result = 0, power = 1;
            for (var i = 0; i < p.Length; i++)
            {
                result += p[p.Length - 1 - i] * power;
                power *= C;
            }
            return result;
        }
        
        static byte[] rotate(byte[] p, int N) => pattern((x, y) => p[N - 1 - y + x * N], N); //Rota 90 º en sentido contrario a las agujas del reloj
        static byte[] reflect(byte[] p, int N) => pattern((x, y) => p[N - 1 - x + y * N], N); //flippity floppity 
        static byte[] pattern(Func<int, int, byte> f, int N)
        {
            var result = new byte[N * N];
            
            for (var y = 0; y < N; y++) 
                for (var x = 0; x < N; x++) 
                    result[x + y * N] = f(x, y);
            return result;
        }

             
    }

}
