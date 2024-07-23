using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snake{
    public static class Utils{

        public static Vector2 Pop(this List<Vector2> list)
        {
            Vector2 item = list[0];
            list.RemoveAt(0);
            return item;
        }

        public static void Add(this List<Vector2> list, int i, int j){
            list.Add(new Vector2(i, j));
        }

        public static void Add(this List<Vector2> list, (int i, int j) value){
            list.Add(value.i, value.j);
        }

        public static bool Contains (this List<Vector2> list, (int i, int j) value){
            return list.Contains(new(value.i, value.j));
        }

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
            return tile.name == "Rubble";
        }



    }

    public static class timeTestUtils{
        public static bool Testing = false;

        public static void printTimeStamp(string s = ""){
            if(!Testing) return;
            string timestamp = DateTime.UtcNow.ToString("HH:mm:ss.fff");
            Debug.Log($"{s} Time: {timestamp}");
        }
        public static void printTimeDifference(DateTime start, DateTime end, string TaskName = ""){
            if(!Testing) return;
            var timestamp = end.Subtract(start).TotalMilliseconds;
            Debug.Log($"{TaskName} Time: {timestamp} milliseconds");
        }

        public static DateTime getNow(){
            return DateTime.UtcNow;
        }
    }
}