using System.Collections.Generic;
using UnityEngine;

namespace WFC.Texture
{
    public class WfcTextureModel : WfcModel
    {
        public Color[] tiles;
        
        public WfcTextureModel(int nTiles, int tileSize, bool periodic, bool ground, double[] weights,
            Color[] tiles, List<byte[]> patterns, int[][][] propagator) : 
            base (nTiles, tileSize, periodic, ground, weights, patterns, propagator)
        {
            this.tiles = tiles;
        }

    }
}