using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFC.Tilemap.Overlap
{
    [Serializable]
    public class Model : WFC.Model
    {
        public TileBase[] tileBases;
        public List<byte[]> patterns{get; protected set;}

        public Model(TileBase[] tileBases, List<byte[]> patterns, int[][][] propagator, int n, int nTiles, bool periodic, double[] weights, bool ground  = false):
            base (nTiles, n, periodic, ground, weights, propagator)
        {
            this.tileBases = tileBases;
            this.patterns=patterns;
        }
    }
}