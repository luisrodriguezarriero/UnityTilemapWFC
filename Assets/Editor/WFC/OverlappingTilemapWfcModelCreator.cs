using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

namespace WFC_Unity_Luyso
{
    public class OverlappingTilemapWfcModelCreator : TilemapWfcModelCreator
    {
        TileBase[] tiles;
        List<byte[]> patterns;
        public OverlappingTilemapWfcModelCreator(Tilemap inputImage, int patternSize, bool periodic, int simmetry) :
            base(patternSize, periodic)
        {

            TileBase[] allTiles = GetTileList(inputImage);
            byte[] tilesAsBytes = TilesAsBytes(allTiles);
            (int SX, int SY) = InputSize(inputImage);

            PatternList p = new PatternList(tilesAsBytes, SX, SY, N, simmetry, periodic, inputImage.GetUsedTilesCount());

            T = p.T;
            patterns = p.patterns;
            weights = p.weights;

            InitPropagator();   
        }

        private byte[] TilesAsBytes(TileBase[] allTiles)
        {
            List<TileBase> distinctTiles = new List<TileBase>();
            byte[] inputAsInts = new byte[allTiles.Length];
            for (int i = 0; i < allTiles.Length; i++)
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
        protected void InitPropagator()
        {
            propagator = new int[4][][];
            for (int d = 0; d < 4; d++)
            {
                propagator[d] = new int[T][]; // Número de Patrones
                for (int t = 0; t < T; t++)
                {
                    List<int> list = new();
                    for (int t2 = 0; t2 < T; t2++)
                        if (agrees(patterns[t], patterns[t2], dx[d], dy[d], N)) list.Add(t2);
                    propagator[d][t] = list.ToArray();

                    list.Clear();
                }
            }
        }
        
        static bool agrees(byte[] p1, byte[] p2, int dx, int dy, int N)
        {
            int xmin = dx < 0 ? 0 : dx, xmax = dx < 0 ? dx + N : N, ymin = dy < 0 ? 0 : dy, ymax = dy < 0 ? dy + N : N;
            for (int y = ymin; y < ymax; y++) for (int x = xmin; x < xmax; x++) if (p1[x + N * y] != p2[x - dx + N * (y - dy)]) return false;
            return true;
        }

        private (int, int) InputSize(Tilemap inputImage)
        {
            BoundsInt inputBounds = inputImage.cellBounds;
            int SX = inputBounds.xMax - inputBounds.xMin;
            int SY = inputBounds.yMax - inputBounds.yMin;
            return (SX, SY);
        }

        private TileBase[] GetTileList(Tilemap inputImage)
        {
            inputImage.CompressBounds();
            BoundsInt inputBounds = inputImage.cellBounds;
            TileBase[] allTiles = inputImage.GetTilesBlock(inputBounds);
            return allTiles;
        }

        public override void Save(Tilemap output, int[] observed, int MX, int MY)
        {
            if (observed[0] >= 0)
            {
                for (int y = 0; y < MY; y++)
                {
                    int dy = y < MY - N + 1 ? 0 : N - 1;
                    for (int x = 0; x < MX; x++)
                    {
                        int dx = x < MX - N + 1 ? 0 : N - 1;
                        try
                        {
                            output.SetTile(new Vector3Int(x, y, 0), tiles[patterns[observed[x - dx + (y - dy) * MX]][dx + dy * N]]);
                        }
                        catch(ArgumentOutOfRangeException e)
                        {
                            Debug.Log($"Valor inválido {observed[x - dx + (y - dy) * MX]}");
                        }
                    }
                }
            }
            //Debug.Log(ToString());
        }

        private TilemapWfcModel model;
        public void exportModel(string name = "overlapModel")
        {
            TilemapWfcModel model = ScriptableObject.CreateInstance<TilemapWfcModel>();
            model.setValues(tiles, patterns, propagator, T, N, periodic, ground, weights);

            model.ExportToJson(name);

        }

        public TilemapWfcModel getModel()
        {
            model = ScriptableObject.CreateInstance<TilemapWfcModel>();
            model.setValues(tiles, patterns, propagator, T, N, periodic, ground, weights);
            return model;
        }
    }
}
