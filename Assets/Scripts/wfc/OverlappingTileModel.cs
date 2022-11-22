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
        int SX = bounds.size.x, SY = bounds.size.y;

        int nTiles = tilemap.GetUsedTilesCount();
        weights = new double[tilemap.GetUsedTilesCount() + 1];

        propagator = new int[4][][];

        patterns = new();
        Dictionary<String, int> patternIndices = new();
        List<double> weightList = new();
        T = tilemap.GetUsedTilesCount() + 1;
        bool[][][] relations = new bool [T][][];

        static TileBase[] pattern(Func<int, int, TileBase> f, int N)
        {
            TileBase[] result = new TileBase[N*N];
            for (int x=0; x<N; x++)for (int y=0; y<N; y++) result[x + y * N] = f(x, y);
            return result;
        }

        int xmax = periodicInput ? SX : SX - N + 1;
        int ymax = periodicInput ? SY : SY - N + 1;
        for (int y = 0; y < ymax; y++) for (int x = 0; x < xmax; x++)
            {
            TileBase[] p = pattern((dx, dy) => allTiles[(x + dx) % SX + (y + dy) % SY * SX], N);
            String hash = Helper.HashTilePattern(p);
            if (patternIndices.TryGetValue(hash, out int index)) weightList[index] = weightList[index] + 1;
            else
            {
                patternIndices.Add(hash, weightList.Count);
                weightList.Add(1.0);
                patterns.Add(p);
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
        int[] tilemapData = new int[MX * MY];
        //TODO
        target = new Tilemap();

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

