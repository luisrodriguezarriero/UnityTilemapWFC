using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static WFC.Helper;

namespace WFC
{
    public class WfcTextureCreator : WfcCreator
    {
        public Texture2D input;
        public Color32[] tiles;
        private TextureWfcModel model = null;

        public WfcTextureCreator(Texture2D input, int tileSize, bool periodicInput, bool periodic, int symmetry, bool ground, Heuristic heuristic)
        {
            this.input = input;
            this.tileSize = tileSize;
            this.periodicInput = periodicInput;
            this.periodic = periodic;
            this.simmetry = symmetry;
            this.ground = ground;
            this.heuristic = heuristic;
        }

        public IEnumerator CreateWfcModel()
        {
            var pixelData = input.GetPixelData<Color32>(0).ToArray();

            var p = new PatternList(ColorsAsBytes(pixelData), input.width, input.height, tileSize, simmetry,
                periodicInput, tiles.Length);

            var propagator = InitPropagator(p.patterns, p.T, tileSize);
            
            this.model = new TextureWfcModel(p.T, tileSize, periodic, ground, p.weights, tiles, p.patterns, propagator);
            yield return null;
        }

        private byte[] ColorsAsBytes(Color32[] allTiles)
        {
            var distinctTiles = new List<Color32>();
            var inputAsInts = new byte[allTiles.Length];
            for (var i = 0; i < allTiles.Length; i++)
            {
                if (!distinctTiles.Contains(allTiles[i]))
                {
                    distinctTiles.Add(allTiles[i]);
                }
                inputAsInts[i] = (byte)distinctTiles.IndexOf(allTiles[i]);
            }
            tiles = distinctTiles.ToArray();
            return inputAsInts;
        }
        
        protected int[][][] InitPropagator(List<byte[]> patterns, int T, int N)
        {
            var propagator = new int[4][][];
            for (var d = 0; d < 4; d++)
            {
                propagator[d] = new int[T][]; // Número de Patrones
                for (var t = 0; t < T; t++)
                {
                    List<int> list = new();
                    for (var t2 = 0; t2 < T; t2++)
                        if (agrees(patterns[t], patterns[t2], dx[d], dy[d], N)) list.Add(t2);
                    propagator[d][t] = list.ToArray();

                    list.Clear();
                }
            }
            return propagator;
        }

        public override void CreateModel()
        {
            throw new System.NotImplementedException();
        }

        public override void Solve(){
            var solver = new WfcSolver(model, this.outputWidth, this.outputHeight, this.heuristic, this.periodic);
            GenerateSeed();
            if (solver.Run(seed, maxIterations > 0 ? maxIterations : int.MaxValue))
                {}
            else Debug.Log("Fail");
        }

        public override void CreateMap(){
            var wfcModel = CreateWfcModel();
            Solve();
        }

        public override bool modelExists(){
            return this.model!=null;
        }

        public override bool hasInput(){
            return this.input!=null;
        }
    }
}


