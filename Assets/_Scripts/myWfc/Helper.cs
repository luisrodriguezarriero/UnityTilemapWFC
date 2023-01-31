using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    static class Helper
    {
        public static int Random(this double[] weights, double r)
        {
            double sum = 0;
            for (int i = 0; i < weights.Length; i++) sum += weights[i];
            double threshold = r * sum;

            double partialSum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                partialSum += weights[i];
                if (partialSum >= threshold) return i;
            }
            return 0;
        }
    }



