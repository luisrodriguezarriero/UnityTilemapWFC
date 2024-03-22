using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFC.TiledWFC
{
    [Serializable]
    public class TilemapWfcModel : WfcModel
    {
        public TileBase[] tileBases;

        public TilemapWfcModel(TileBase[] tileBases, List<byte[]> patterns, int[][][] propagator, int n, int nTiles, bool periodic, double[] weights, bool ground  = false):
            base (nTiles, n, periodic, ground, weights, patterns, propagator)
        {
            this.tileBases = tileBases;
        }
    }
}