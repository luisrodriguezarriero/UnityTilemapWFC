using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;

namespace fire
{
    public class OverModel : Model
    {
        private Tilemap inputImage;
        private Tilemap outputImage;

        private TileBase[] tiles;
        List<byte[]> patterns;
        public OverModel(Tilemap inputImage, Tilemap outputImage, int patternSize, int outputWidth, int outputHeight, bool periodic, int simmetry, Heuristic heuristicType) :
            base(outputWidth, outputHeight, patternSize, periodic, heuristicType)
        {
            this.inputImage = inputImage;
            this.outputImage = outputImage;

            TileBase[] allTiles = GetTileList();
            byte[] tilesAsBytes = TilesAsBytes(allTiles);
            (int SX, int SY) = InputSize();

            LuysoWFC.PatternListCreator p = new LuysoWFC.PatternListCreator(tilesAsBytes, SX, SY, N, simmetry, periodic, inputImage.GetUsedTilesCount());

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

        private (int, int) InputSize()
        {
            BoundsInt inputBounds = inputImage.cellBounds;
            int SX = inputBounds.xMax - inputBounds.xMin;
            int SY = inputBounds.yMax - inputBounds.yMin;
            return (SX, SY);
        }

        private Boolean CheckEmptySpaces()
        {
            return false; //TODO
        }

        private TileBase[] GetTileList()
        {
            inputImage.CompressBounds();
            BoundsInt inputBounds = inputImage.cellBounds;
            TileBase[] allTiles = inputImage.GetTilesBlock(inputBounds);
            return allTiles;
        }

        public override void Save(string filename)
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
                            outputImage.SetTile(new Vector3Int(x, y, 0), tiles[patterns[observed[x - dx + (y - dy) * MX]][dx + dy * N]]);
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

        void InitHeuristic()
        {
            
        }
        private void InitWave()
        {
            wave = new bool[MX * MY][];
            for (int i = 0; i < wave.Length; i++)
            {
                wave[i] = new bool[T];
            }
        }
        private void InitStack()
        {

        }


        protected bool isNodeExcluded(int n)
        {
            return (!periodic && (n % MX + N > MX || n / MX + N > MY));
        }


    }
}
