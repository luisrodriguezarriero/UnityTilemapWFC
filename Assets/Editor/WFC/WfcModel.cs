using System;
using System.Collections.Generic;

[Serializable]
public class WfcModel
{
    public List<byte[]> patterns{get; protected set;}
    internal int[][][] propagator;
    internal int nTiles, n;
    internal bool periodic, ground;
    public double[] weights;
            
    public WfcModel(int nTiles, int tileSize, bool periodic, bool ground, double[] weights,
        List<byte[]> patterns, int[][][] propagator)
        {
        this.nTiles = nTiles;
        n = tileSize;
        this.periodic = periodic;
        this.ground = ground;
        this.weights = weights;
        this.propagator = propagator;
        this.patterns = patterns;
    }
}