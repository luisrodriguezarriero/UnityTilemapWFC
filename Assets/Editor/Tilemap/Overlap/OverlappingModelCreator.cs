using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using static WFC.Utilities;
using System.Data.SqlTypes;

namespace WFC.Tilemap.Overlap
{
    public class OverlappingModelCreator : WFC.ModelCreator
    {
        protected TileBase[] tiles;
        protected readonly List<byte[]> patterns;
        public OverlappingModelCreator(
            UnityEngine.Tilemaps.Tilemap inputImage, int patternSize, bool periodic, int symmetry, bool ground) :
            base(patternSize, periodic, ground)
        {        
            var allTiles = GetTileList(inputImage);
            var tilesAsBytes = TilesAsBytes(allTiles);
            var (sx, sy) = InputSize(inputImage);

            var patternList = new PatternList(tilesAsBytes, sx, sy, N, symmetry, periodic, inputImage.GetUsedTilesCount());

            patterns = patternList.patterns;
            weights = patternList.weights;
            this.T=patternList.T;

            InitPropagator(patterns, T);   
            model = new OverlappingModel(tiles, patterns, propagator, N, patternList.T, periodic, weights);
        }

        private byte[] TilesAsBytes(TileBase[] allTiles)
        {
            if (allTiles == null) throw new ArgumentNullException(nameof(allTiles));

            var distinctTiles = new List<TileBase>();
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

        private void InitPropagator(List<byte[]> patterns, int T)
        {
            var propagator = new int[4][][];
            for (var d = 0; d < 4; d++)
            {
                propagator[d] = new int[T][];
                for (var t = 0; t < T; t++)
                {
                    List<int> list = new List<int>();
                    for (var t2 = 0; t2 < T; t2++)
                        if (arePatternsHardCompatible(patterns[t], patterns[t2], dx[d], dy[d], N)) list.Add(t2);
                    propagator[d][t] = list.ToArray();

                    list.Clear();
                }
            }
            this.propagator=propagator;
        }

        private (int, int) InputSize(UnityEngine.Tilemaps.Tilemap inputImage)
        {
            var inputBounds = inputImage.cellBounds;
            var sx = inputBounds.xMax - inputBounds.xMin;
            var sy = inputBounds.yMax - inputBounds.yMin;
            return (sx, sy);
        }

        private TileBase[] GetTileList(UnityEngine.Tilemaps.Tilemap inputImage)
        {
            inputImage.CompressBounds();
            var inputBounds = inputImage.cellBounds;
            var allTiles = inputImage.GetTilesBlock(inputBounds);
            return allTiles;
        }

        public override void Save(UnityEngine.Tilemaps.Tilemap output, int[] observed, int mx, int my)
        {
            if (observed[0] >= 0)
            {
                for (var y = 0; y < my; y++)
                {
                    var dy = y < my - N + 1 ? 0 : N - 1;
                    for (var x = 0; x < mx; x++)
                    {
                        var dx = x < mx - N + 1 ? 0 : N - 1;
                        try
                        {
                            TileBase t = tiles[patterns[observed[x - dx + (y - dy) * mx]][dx + dy * N]];
                            Vector3Int location = new Vector3Int(x, y, 0);
                            output.SetTile(location, t);
                        }
                        catch(ArgumentOutOfRangeException e)
                        {
                            Debug.Log($"Error in value: {observed[x - dx + (y - dy) * mx]}");
                            Debug.Log(e.Message);
                        }
                    }
                }
            }
        }

        public OverlappingModel model{get; private set;}
    }
}
