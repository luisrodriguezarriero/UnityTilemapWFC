using UnityEngine.Tilemaps;
using System;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
#if UNITY_EDITOR
    using UnityEditor;
    using System.Collections.Generic;
#endif

namespace WFC
{
    public class OverlapEditor : WfcCreator
    {
        public Tilemap inputGrid;
        public GameObject outputPrefab;
        private Tilemap outputGrid;

        public String savedName = "";
        [Tooltip("")]
        public String modelName = "";

        public bool overrideOldMap;

        OverlappingTilemapWfcModelCreator wfc;
        private TilemapWfcModel model;

        public override void CreateModel()
        {
            checkSimmetry();
            
            if (inputGrid)
            {
                wfc = new OverlappingTilemapWfcModelCreator(inputGrid, tileSize,
                    periodic, simmetry, false);
                model = wfc.GetModel();
            }
        }
        public override void Solve()
        {
            ClearOutput();
            var solver = new WfcSolver(model, this.outputWidth, this.outputHeight, this.heuristic, periodic);
            if (solver.Run(seed, maxIterations > 0 ? maxIterations : int.MaxValue))
                wfc.Save(outputGrid, solver.Result, outputWidth, outputHeight);
            else Debug.Log("Fail");
        }

        public void SaveTilemap()
        {
            var savedTilemapName = savedName.Length > 0 ? savedName : "savedTilemap";
            var dir = "Assets/SavedTilemaps/" + savedTilemapName + ".prefab";
            try
            {
                SaveTilemapPrefab(dir);
            }
            catch (ArgumentException e)
            {
                Debug.Log(e.Message + ", Creating folder...");
            #if UNITY_EDITOR
                AssetDatabase.CreateFolder("Assets", "SavedTilemaps");
                SaveTilemapPrefab(dir);
            #endif
            }

        }

        
        private void SaveTilemapPrefab(String savedPrefabName)
        {
            var data= JsonUtility.ToJson(model);
            Debug.Log(data);
            var file = File.CreateText($@"Assets/Tilemaps/{savedPrefabName}.json");
            var serializer = new JsonSerializer();
            
            //serialize object directly into file stream
            serializer.Serialize(file, data);
        }

        internal void SaveModel()
        {
            wfc.ExportModel(modelName);
        }

        private void CreateOutput()
        {
            var outputTilemapGameObject = Instantiate(outputPrefab, this.gameObject.transform, true);
            outputGrid = outputTilemapGameObject.GetComponent<Tilemap>();
        }

        private void ClearOutput()
        {
            if (outputGrid && overrideOldMap)
            {
                outputGrid.ClearAllTiles();
            }
            else CreateOutput();
        }

        public override bool modelExists()
        {
            return this.model!=null;
        }

        public override bool hasInput()
        {
            return this.inputGrid!=null;
        }

        public override void CreateMap()
        {
            CreateModel();
            Solve();
        }
    }
}
