// Copyright (C) 2016 Maxim Gumin, The MIT License (MIT)

using System;
namespace LuysoWFC
{
    public abstract class LuysoHeuristic
    {
        protected int nNodes;

        public int[] sumsOfOnes;

        public int[] observed;
        public int observedSoFar;

        protected LuysoHeuristic(int nNodes)
        {
            this.nNodes = nNodes;

            sumsOfOnes = new int[nNodes];

            observed = new int[nNodes];
            observedSoFar = 0;
        }

        public abstract int NextUnobservedNode(Random random, Func<int, bool> isNodeExcluded);

        public virtual void Ban(int i, int t)
        {
            sumsOfOnes[i] -= 1;
        }

        public virtual void Clear()
        {
            for (int i = 0; i < nNodes; i++)
            {
                observed[i] = -1;
                sumsOfOnes[i] = nNodes;
            }

            observedSoFar = 0;
        }
    }
}
