
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace WFC_Unity_Luyso
{
    public abstract class TilemapWfcModelCreator
    {
        protected int[][][] propagator;
        
        protected int T, N;
        protected bool periodic, ground;
        
        public double[] weights;

        protected TilemapWfcModelCreator(int N, bool periodic, bool ground = false)
        {
            this.N = N;
            this.periodic = periodic;
            this.ground = ground;
        }

        public int[][][] Propagator => propagator;

        public int nTiles => T;

        public int tileSize => N;

        public bool Periodic => periodic;

        public bool Ground => ground;

        public double[] Weights => weights;
        public abstract void Save(Tilemap outout, int[] observed, int MX, int MY);
    }
}
