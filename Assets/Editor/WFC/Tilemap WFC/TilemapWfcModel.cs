using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Tilemaps;
using Newtonsoft.Json.Linq;

namespace WFC
{
    public class TilemapWfcModel : WfcModel
    {
        public TilemapWfcModel(TileBase[] tileBases, List<byte[]> patterns, int[][][] propagator, int n, int nTiles, bool periodic, double[] weights, bool ground  = false):
            base (nTiles, n, periodic, ground, weights, patterns, propagator)
        {
            this.tileBases = tileBases;
        }

        private List<byte[]> PatternListParser(byte[][] patternsArray)
        {
            var patternList = new List<byte[]>(); 
            foreach(var pattern in patternsArray)
            {
                patternList.Add(pattern);
            }
            return patternList;
        }


        public void ExportToJson(string savedModelName)
        {
            var jModel = JModelCreator();
            var path = $"Assets/Editor/WFC/Models/{savedModelName}.json";
            File.WriteAllText(path, jModel.ToString());
        }
        private JObject JModelCreator()
        {
            var jModel = new JObject();
            jModel["nTiles"] = this.nTiles;
            jModel["tileSize"] = this.n;
            jModel["periodic"] = this.periodic;
            jModel["ground"] = this.ground;
            jModel.Add(new JProperty("weights", this.weights));
            jModel["tileBases"] = tileBases.Length;
            jModel["patterns"] = PatternsToJArray();
            jModel["propagator"] = PropagatorToJArray();
            return jModel;
        }
     
        private JArray PropagatorToJArray()
        {
            var jPropagator = new JArray();
            foreach (var ar1 in propagator)
            {
                var jProp1 = new JArray();
                foreach (var ar2 in ar1)
                {
                    var jProp2 = new JArray();
                    foreach (var ar3 in ar2)
                    {
                        jProp2.Add(ar3);
                    } jProp1.Add(jProp2);
                }
                jPropagator.Add(jProp1);
            }
            return jPropagator;
        }
        private JArray PatternsToJArray()
        {
            var patternsJson = new JArray();
            foreach (var pattern in patterns)
            {
                var patternJson = new JArray();
                foreach (var b in pattern)
                {
                    patternJson.Add(b);
                }
                patternsJson.Add(patternJson);
            }
            return patternsJson;
        }

        public TileBase[] tileBases;
        

    }
}