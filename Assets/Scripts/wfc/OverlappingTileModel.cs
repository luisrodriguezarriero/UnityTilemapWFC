// Copyright (C) 2016 Maxim Gumin, The MIT License (MIT)

using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OverlappingTileModel  : Model
{
    List<TileBase> tiles;

    List<string> tilenames;
    bool blackBackground;
    Tilemap tilemap; 
    List<TileBase[]> patterns;
    List<int[]> patternIndexes;
    Tilemap target;

    public OverlappingTileModel(Tilemap tilemap, bool periodicInput, int width, int height, int N,
                            bool periodic, bool blackBackground, bool ground, Heuristic heuristic) : 
                            base(width, height, 1, periodic, heuristic)
    {
        this.blackBackground = blackBackground;
        this.tilemap = tilemap;

        tilemap.orientation = Tilemap.Orientation.XY;
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        TileBase[] sample = new TileBase[allTiles.Length];
        tiles = new List<TileBase>();
        for (int i = 0; i < sample.Length; i++)
        {
            int color = allTiles[i];
            int k = 0;
            for (; k < colors.Count; k++) if (colors[k] == color) break;
            if (k == colors.Count) colors.Add(color);
            sample[i] = (byte)k;
        }

        int SX = bounds.size.x, SY = bounds.size.y;

        int nTiles = tilemap.GetUsedTilesCount();
        weights = new double[tilemap.GetUsedTilesCount() + 1];

        propagator = new int[4][][];

        patterns = new();
        Dictionary<String, int> patternIndices = new();
        List<double> weightList = new();
        T = tilemap.GetUsedTilesCount() + 1;
        bool[][][] relations = new bool [T][][];

        static (TileBase[], int[]) pattern (Func<int, int, int> f, Func<int, TileBase> g, int N)
        {
            (TileBase[] pattern, int[] index)  result = (new TileBase[N*N], new int[N*N]);
            for (int x = 0; x < N; x++) for (int y = 0; y < N; y++)
                {
                    int i = f(x, y);
                    result.pattern[x + y * N] = g(i);
                    result.index[x + y * N] = i;
                }
            return result;
        }

        int xmax = periodicInput ? SX : SX - N + 1;
        int ymax = periodicInput ? SY : SY - N + 1;
        for (int y = 0; y < ymax; y++) for (int x = 0; x < xmax; x++)
            {
            (TileBase[] pattern, int[] index) p = pattern((dx, dy) => (x + dx) % SX + (y + dy) % SY * SX, i => allTiles[i],  N);
            String hash = Helper.HashTilePattern(p.pattern);
            if (patternIndices.TryGetValue(hash, out int index)) weightList[index] = weightList[index] + 1;
            else
            {
                patternIndices.Add(hash, weightList.Count);
                weightList.Add(1.0);
                patterns.Add(p.pattern);
                patternIndexes.Add(p.index);
            }
        }
        weights = weightList.ToArray();
        this.ground = ground;

        static bool agrees(TileBase[] p1, TileBase[] p2, int dx, int dy, int N)
        {
            int xmin = dx < 0 ? 0 : dx, xmax = dx < 0 ? dx + N : N, ymin = dy < 0 ? 0 : dy, ymax = dy < 0 ? dy + N : N;
            for (int y = ymin; y < ymax; y++) for (int x = xmin; x < xmax; x++) if (p1[x + N * y].name != p2[x - dx + N * (y - dy)].name) return false;
            return true;
        };

        propagator = new int[4][][];
        for (int d = 0; d < 4; d++)
        {
            propagator[d] = new int[T][]; // Número de Patrones
            for (int t = 0; t < T; t++)
            {
                List<int> list = new();
                for (int t2 = 0; t2 < T; t2++) if (agrees(patterns[t], patterns[t2], dx[d], dy[d], N)) list.Add(t2);
                propagator[d][t] = new int[list.Count];
                for (int c = 0; c < list.Count; c++) propagator[d][t][c] = list[c];
            }
        }
    }

    public override void Save(string filename)
    {
        TileBase[] bitmap = new TileBase[MX * MY];
        if (observed[0] >= 0)
        {
            for (int y = 0; y < MY; y++)
            {
                int dy = y < MY - N + 1 ? 0 : N - 1;
                for (int x = 0; x < MX; x++)
                {
                    int dx = x < MX - N + 1 ? 0 : N - 1;
                    bitmap[x + y * MX] = tiles[patternIndexes[observed[x - dx + (y - dy) * MX]][dx + dy * N]];
                }
            }
        }
        else
        {
            for (int i = 0; i < wave.Length; i++)
            {
                int contributors = 0;
                TileBase result;
                int x = i % MX, y = i / MX;
                for (int dy = 0; dy < N; dy++) for (int dx = 0; dx < N; dx++)
                    {
                        int sx = x - dx;
                        if (sx < 0) sx += MX;

                        int sy = y - dy;
                        if (sy < 0) sy += MY;

                        int s = sx + sy * MX;
                        if (!periodic && (sx + N > MX || sy + N > MY || sx < 0 || sy < 0)) continue;
                        for (int t = 0; t < T; t++) if (wave[s][t])
                            {
                                contributors++;
                                result = allTiles[patternIndexes[t][dx + dy * N]];
                            }
                    }
                bitmap[i] = unchecked((int)0xff000000 | ((r / contributors) << 16) | ((g / contributors) << 8) | b / contributors);
            }
        }
        //BitmapHelper.SaveBitmap(bitmap, MX, MY, filename);
        ;    }

    public string TextOutput()
    {
        var result = new System.Text.StringBuilder();
        for (int y = 0; y < MY; y++)
        {
            for (int x = 0; x < MX; x++) result.Append($"{tilenames[observed[x + y * MX]]}, ");
            result.Append(Environment.NewLine);
        }
        return result.ToString();
    }
}

