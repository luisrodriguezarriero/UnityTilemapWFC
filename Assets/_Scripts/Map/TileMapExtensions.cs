using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.Editor
{
    public static class TilemapExtensions
    {
        public static TileBase[] getTileList(this Tilemap tilemap)
        {
            List<TileBase> tileList = new List<TileBase>();
        
            var tileArray = tilemap.GetTilesBlock(tilemap.cellBounds);

            foreach (TileBase tile in tileArray)
            {
                if(!tileList.Contains(tile))tileList.Add(tile);
            }

            return tileList.ToArray();
        }
    }
}