using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using static WFC.Helper;


namespace WFC
{
    public class SnakeMapCreator : OverlappingTilemapWfcModelCreator
    {

        public SnakeMapCreator(Tilemap inputImage, int patternSize, bool periodic = false, int symmetry = 1, bool ground = false): 
            base (inputImage, patternSize, periodic, symmetry, ground)
        {}
        
        public void Save(Tilemap output, int[] observed)
        {
            var bounds = output.cellBounds;
            int xSize = bounds.size.x-2, ySize=bounds.size.y-2;
            if (observed[0] < 0) return;
            for (var y = 0; y < ySize; y++)
            {
                var dy = y < ySize - N + 1 ? 0 : N - 1;
                for (var x = 0; x < xSize; x++)
                {
                    var dx = x < xSize - N + 1 ? 0 : N - 1;
                    try
                    {
                        output.SetTile(new Vector3Int(x+bounds.xMin+1, y+bounds.yMin+1, 0), tiles[patterns[observed[x - dx + (y - dy) * xSize]][dx + dy * N]]);
                    }
                    catch(ArgumentOutOfRangeException e)
                    {
                        Debug.Log($"Valor inválido {observed[x - dx + (y - dy) * xSize]}");
                        Debug.Log(e.Message);
                    }
                }
            }
            //Debug.Log(ToString());
        }

    }
}