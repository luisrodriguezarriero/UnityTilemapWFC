using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace WFC_Unity_Luyso
{
    public class TilemapWfcModel: ScriptableObject
    {
        public void setValues(TileBase[] tiles, List<byte[]> patterns, int[][][] propagator, int t, int n, bool periodic, bool ground, double[] weights)
        {
            this.tiles = tiles;
            this.patterns = patterns;
            this.propagator = propagator;
            this.nTiles = t;
            this.n = n;
            this.periodic = periodic;
            this.ground = ground;
            this.weights = weights;
        }

        public void ExportToJson(string savedModelName)
        {
        
            string data= JsonUtility.ToJson(this);
            Debug.Log(data);
            string path = $@"Assets/Tilemaps/{savedModelName}.json";
            
            StreamWriter streamWriter = new StreamWriter(path);

            JsonTextWriter jsonWriter = new JsonTextWriter(streamWriter);

            JObject jsonObject = JObject.Parse(data);
            jsonObject.WriteTo(jsonWriter);
        }
        public TileBase[] tiles;
        private List<byte[]> patterns;
        private int[][][] propagator;

        [FormerlySerializedAs("N")] public int n;
        private bool periodic, ground;
        private double[] weights;

        public TileBase[] Tiles => tiles;

        public List<byte[]> Patterns => patterns;

        public int[][][] Propagator => propagator;

        public int nTiles { get; private set; }

        public bool Periodic => periodic;

        public bool Ground => ground;

        public double[] Weights => weights;
    }
}