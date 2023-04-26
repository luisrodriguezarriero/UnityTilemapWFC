using System.Collections.Generic;
using System;

namespace LuysoWFC
{
    public class PatternListCreator
    {
        public List<byte[]> patterns;
        public int T;
        public double[] weights;
        public PatternListCreator(byte[] sample, int SX, int SY, int N, int simmetry, bool periodicInput, int tilesCount)
        {
            int xmax = periodicInput ? SX : SX - N + 1;
            int ymax = periodicInput ? SY : SY - N + 1;
            Dictionary<long, int> patternIndices = new();

            patterns = new();

            List<double> weightList = new();


            static byte[] rotate(byte[] p, int N) => pattern((x, y) => p[N - 1 - y + x * N], N); //Rota 90 º en sentido contrario a las agujas del reloj
            static byte[] reflect(byte[] p, int N) => pattern((x, y) => p[N - 1 - x + y * N], N); //flippity floppity 
            static byte[] pattern(Func<int, int, byte> f, int N)
            {
                byte[] result = new byte[N * N];
                for (int y = 0; y < N; y++) for (int x = 0; x < N; x++) result[x + y * N] = f(x, y);
                return result;
            }

            for (int y = 0; y < ymax; y++)
                for (int x = 0; x < xmax; x++)
                {
                    byte[][] ps = new byte[8][];

                    ps[0] = pattern((dx, dy) => sample[(x + dx) % SX + (y + dy) % SY * SX], N);
                    ps[1] = reflect(ps[0], N);
                    ps[2] = rotate(ps[0], N);
                    ps[3] = reflect(ps[2], N);
                    ps[4] = rotate(ps[2], N);
                    ps[5] = reflect(ps[4], N);
                    ps[6] = rotate(ps[4], N);
                    ps[7] = reflect(ps[6], N);

                    for (int k = 0; k < simmetry; k++)
                    {
                        byte[] p = ps[k];
                        long h = hash(p, tilesCount);
                        if (patternIndices.TryGetValue(h, out int index)) weightList[index] = weightList[index] + 1;
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
        static long hash(byte[] p, int C)
        {
            long result = 0, power = 1;
            for (int i = 0; i < p.Length; i++)
            {
                result += p[p.Length - 1 - i] * power;
                power *= C;
            }
            return result;
        }
}

}
