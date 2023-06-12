using System.Collections.Generic;

public class WfcModel
{
    internal List<byte[]> patterns;
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