using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Serialization;
using static WFC_Unity_Luyso.Helper;
#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace WFC_Unity_Luyso
{
    public class OverlapEditor : MonoBehaviour
    {
        public Tilemap inputGrid;
        [FormerlySerializedAs("outputGrid")] public GameObject outputPrefab;
        private Tilemap outputGrid;

        [Tooltip("Recommended values between 2 and 4")]
        public int patternSize;
        [Tooltip("How many times algorithm will try creating the output before quiting")]
        public int maxIterations;
        [Tooltip("Output image width")]
        public int outputWidth = 5;
        [Tooltip("Output image height")]
        public int outputHeight = 5;
        [Tooltip("Between 1 and 8, any other value will be tweaked")]
        public int simmetry = 1;
        [Tooltip("")]
        public bool periodic = false;
        [Tooltip("")]
        public String savedName = "";
        [Tooltip("")]
        public String modelName = "";

        public bool overrideOldMap;
        
        public Heuristic heuristicType;

        private int seed;
        OverlappingTilemapWfcModelCreator wfc;
        [SerializeField] internal TilemapWfcModel model;
        public void CreateWFC()
        {
            if (simmetry < 1) simmetry = 1;
            if (simmetry > 8) simmetry = 8;
            
            if (inputGrid)
            {
                wfc = new OverlappingTilemapWfcModelCreator(inputGrid, patternSize,
                    periodic, simmetry);
                model = wfc.getModel();
            }
        }
        public void SolveWFC()
        {
            clearOutput();
            WfcSolver solver = new WfcSolver(model, this.outputWidth, this.outputHeight);
            if (solver.Run(seed, maxIterations > 0 ? maxIterations : int.MaxValue))
                wfc.Save(outputGrid, solver.result, outputWidth, outputHeight);
            else Debug.Log("Fail");
        }

        public void SaveTilemap()
        {
            String name = savedName.Length > 0 ? savedName : "savedTilemap";
            String dir = "Assets/SavedTilemaps/" + name + ".prefab";
            try
            {
                saveTilemapPrefab(dir);
            }
            catch (ArgumentException e)
            {
            #if UNITY_EDITOR
                AssetDatabase.CreateFolder("Assets", "SavedTilemaps");
                saveTilemapPrefab(dir);
            #endif
            }

        }

        internal void GenerateSeed()
        {
            System.Random s = new System.Random();
            seed = s.Next();

        }
        private void saveTilemapPrefab(String name)
        {
            string _data= JsonUtility.ToJson(model);
            Debug.Log(_data);
            using (StreamWriter file = File.CreateText($@"Assets/Tilemaps/{name}.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
            
                //serialize object directly into file stream
                serializer.Serialize(file, _data);
            }

        }

        internal void SaveModel()
        {
            wfc.exportModel(modelName);
        }

        internal void createOutput()
        {
            GameObject outputTilemapGameObject = Instantiate(outputPrefab);
            outputGrid = outputTilemapGameObject.GetComponent<Tilemap>();
            outputTilemapGameObject.transform.SetParent(this.gameObject.transform);
        }

        internal void clearOutput()
        {
            if (outputGrid && overrideOldMap)
            {
                outputGrid.ClearAllTiles();
            }
            else createOutput();
        }
    }



}
