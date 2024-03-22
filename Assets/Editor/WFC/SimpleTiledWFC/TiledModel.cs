using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFC.Tiled
{
    class TiledModelCreator : WfcModelCreator
    {
        public TiledModelCreator(string name, string subsetName, int width, int height, bool periodic, bool blackBackground) : 
            base(1, periodic, false)
        {
            string data = "";
            if(!GetStringDataFromFileName(name, ref data)) return;

            var action = new List<int[]>();
            //action.AddRange(actionsGenerator("X",  ))

        }

        private bool GetStringDataFromFileName(string name, ref string data)
        {
            try
            {
                data = File.ReadAllText($"{name}.json");
                return true;
            }
            catch(Exception e){
                Debug.Log(e.StackTrace);
                return false;
            }
        }
    
        private int GetCardinality(string sym, ref Func<int, int> rotate, ref Func<int, int> reflect)
        {
            int cardinality = 1;

            switch(sym){
                case "L":  
                    cardinality = 4;     
                    rotate = i => (i + 1) % 4;
                    reflect = i => i % 2 == 0 ? i + 1 : i - 1;
                    break;
                case "T":  
                    cardinality = 4;
                    rotate = i => (i + 1) % 4;
                    reflect = i => i % 2 == 0 ? i : 4 - i;
                    break;
                case "I":
                    cardinality = 2;
                    rotate = i => 1 - i;
                    reflect = i => i;
                    break;
                case "\\":
                    cardinality = 2;
                    rotate = i => 1 - i;
                    reflect = i => 1 - i;
                    break;
                case "F":
                    cardinality = 8;
                    rotate = i => i < 4 ? (i + 1) % 4 : 4 + (i - 1) % 4;
                    reflect = i => i < 4 ? i + 4 : i - 4;
                    break;
                default:
                    rotate = i => i;
                    reflect = i => i;
                    break;
        }
        return cardinality;
        }  

        private List<int[]> actionsGenerator(
            int cardinality, Func<int, int> rotate, Func<int, int> reflect)
        {
            var action = new List<int[]>();
            int[][] map = new int[cardinality][];
            for (int t = 0; t < cardinality; t++)
            {
                map[t] = new int[8];

                map[t][0] = t;
                map[t][1] = rotate(t);
                map[t][2] = rotate(map[t][1]);
                map[t][3] = rotate(map[t][2]);
                map[t][4] = reflect(t);
                map[t][5] = reflect(map[t][1]);
                map[t][6] = reflect(map[t][2]);
                map[t][7] = reflect(map[t][3]);

                for (int s = 0; s < 8; s++) map[t][s] += T;

                action.Add(map[t]);
            }
            return action;
        }

        private void addUniqueTiles(int cardinality){
            
            /*for (int t = 0; t < cardinality; t++)
            {
                int[] bitmap;
                (bitmap, tilesize, tilesize) = BitmapHelper.LoadBitmap($"tilesets/{name}/{tilename} {t}.png");
                tiles.Add(bitmap);
                tilenames.Add($"{tilename} {t}");
            }**/
            

        }
    }
}

