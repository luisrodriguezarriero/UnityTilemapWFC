using System;
using System.Collections.Generic;

namespace WFC{

    [Serializable]
    public class Model
    {
        internal int[][][] propagator;
        internal int nTiles, n;
        internal bool periodic, ground;
        public double[] weights;
                
        public Model(int nTiles, int tileSize, bool periodic, bool ground, double[] weights,
            int[][][] propagator)
            {
            this.nTiles = nTiles;
            n = tileSize;
            this.periodic = periodic;
            this.ground = ground;
            this.weights = weights;
            this.propagator = propagator;
        }
    
    }
}