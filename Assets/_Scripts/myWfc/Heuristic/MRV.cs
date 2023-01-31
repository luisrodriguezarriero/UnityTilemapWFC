// Copyright (C) 2016 Maxim Gumin, The MIT License (MIT)

using System;

namespace LuysoWFC
{
    class MRV : LuysoHeuristic
    {

        public MRV(int nNodes) : base(nNodes)
        { }
        public override int NextUnobservedNode(Random random, Func<int, bool> isNodeExcluded)
        {
            double min = double.MaxValue;
            int argmin = -1;
            double entropy;

            for (int i = 0; i < nNodes; i++)
            {
                if (isNodeExcluded(i)) continue;

                int remainingValues = sumsOfOnes[i];
                entropy = remainingValues;
                if (remainingValues > 1 && entropy < min)
                {
                    min = entropy;
                    argmin = i;
                }
            }
            return argmin;
        }
    }
}
