
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    public static class Utilities
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

        public static readonly Vector3Int 
            vLEFT = new(dx[LEFT], dy[LEFT], 0), 
            vUP = new(dx[UP], dy[UP], 0), 
            vRIGHT = new(dx[RIGHT], dy[RIGHT], 0), 
            vDOWN = new(dx[DOWN], dy[DOWN], 0); 
        public static bool arePatternsHardCompatible(byte[] p1, byte[] p2, int dx, int dy, int N)
        {
            int xmin = dx < 0 ? 0 : dx, xmax = dx < 0 ? dx + N : N, ymin = dy < 0 ? 0 : dy, ymax = dy < 0 ? dy + N : N;
            for (var y = ymin; y < ymax; y++) 
                for (var x = xmin; x < xmax; x++) 
                    if (p1[x + N * y] != p2[x - dx + N * (y - dy)]) return false;
            return true;
        }

        public static void Push(this Stack<(int i, int t)> stack, int i, int t){
            stack.Push((i, t));
        }
    }
    public enum Heuristic { Entropy, MRV, Scanline };
    
}




