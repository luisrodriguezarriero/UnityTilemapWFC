
using System;
using System.Collections.Generic;

namespace WFC
{
    public static class Helper
    {
        public static int Random(this double[] weights, double r)
        {
            double sum = 0;
            for (var i = 0; i < weights.Length; i++) sum += weights[i];
            var threshold = r * sum;

            double partialSum = 0;
            for (var i = 0; i < weights.Length; i++)
            {
                partialSum += weights[i];
                if (partialSum >= threshold) return i;
            }
            return 0;
        }
        
        public static byte[] ToBytes(this int[] numbers, int divider)
        {
            var values = new List<byte>();
            for(var i=0; i<numbers.Length; i++)
            {
                try
                {
                    values.Add((byte)(numbers[i] / divider));
                }
                catch (OverflowException)
                {
                    values.Add(255);
                }
            }
            
            return values.ToArray();
        }
        public static readonly int[] dx = { -1, 0, 1, 0 };
        public static readonly int[] dy = { 0, 1, 0, -1 };
        public static readonly int[] opposite = { 2, 3, 0, 1 };
        public static readonly int LEFT = 0, UP = 1, RIGHT = 2, DOWN = 3; 
        public static bool agrees(byte[] p1,
                                  byte[] p2,
                                  int dx,
                                  int dy,
                                  int N)
        {
            int xmin = dx < 0 ? 0 : dx, xmax = dx < 0 ? dx + N : N, ymin = dy < 0 ? 0 : dy, ymax = dy < 0 ? dy + N : N;
            for (var y = ymin; y < ymax; y++) for (var x = xmin; x < xmax; x++) if (p1[x + N * y] != p2[x - dx + N * (y - dy)]) return false;
            return true;
        }
    }
    public enum Heuristic { Entropy, MRV, Scanline };

    
}




