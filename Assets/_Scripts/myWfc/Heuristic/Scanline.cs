// Copyright (C) 2016 Maxim Gumin, The MIT License (MIT)

using System;

namespace LuysoWFC
{
    class Scanline : LuysoHeuristic
    {
        public Scanline(int nNodes) : base(nNodes)
        { }

        public override int NextUnobservedNode(Random random, Func<int, bool> isNodeExcluded)
        {
            for (int i = observedSoFar; i < nNodes; i++)
            {
                if (isNodeExcluded(i))
                    continue;
                if (sumsOfOnes[i] > 1)
                {
                    observedSoFar = i + 1;
                    return i;
                }
            }
            return -1;
        }


    }
}