using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace Snake
{
    public static class TileBaseExtensions
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
        public static bool isWalkable(this TileBase tile){
            return tile.name == "ground";
        }
    }
}