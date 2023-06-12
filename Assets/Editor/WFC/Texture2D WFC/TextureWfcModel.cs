using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    public class TextureWfcModel : WfcModel
    {
        public Color32[] tiles;
        
        public TextureWfcModel(int nTiles, int tileSize, bool periodic, bool ground, double[] weights,
            Color32[] tiles, List<byte[]> patterns, int[][][] propagator) : 
            base (nTiles, tileSize, periodic, ground, weights, patterns, propagator)
        {
            this.tiles = tiles;
        }

    }
}