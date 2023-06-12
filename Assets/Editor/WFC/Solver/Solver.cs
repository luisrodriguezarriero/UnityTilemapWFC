using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Tilemaps;

namespace WFC
{
    public class Solver : MonoBehaviour
    {
        private TilemapWfcModel model;
        public TileBase[] tiles;
        public int nTilesNeeded;
        private string savedModelName = "patata";
        public GameObject start, end;
        public Grid grid;
        public void importFromJson()
        {
            var path = $"Assets/Editor/WFC/Models/{savedModelName}.json";
#if UNITY_EDITOR
            path = EditorUtility.OpenFilePanel("Overwrite with json", "", "json");
#endif
            if (path.Length != 0)
            {
                using (var file = File.OpenText(path))
                {
                    var serializer = new JsonSerializer();
                    model = (TilemapWfcModel)serializer.Deserialize(file, typeof(TilemapWfcModel));
                    this.nTilesNeeded = model.tileBases.Length;
                }
            }
        }

        public void solve()
        {
            
        }

       
    }
}