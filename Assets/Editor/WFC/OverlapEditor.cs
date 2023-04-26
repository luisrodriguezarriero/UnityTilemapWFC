using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System;
using UnityEditor;
using static UnityEngine.Tilemaps.Tilemap;

namespace fire
{
    public class OverlapEditor : MonoBehaviour
    {
        public Tilemap inputGrid;
        public Tilemap outputGrid;
        [Tooltip("For tiles usualy set to 1. If tile contain just a color can set to higher value")]
        public int patternSize;
        [Tooltip("How many times algorithm will try creating the output before quiting")]
        public int maxIterations;
        [Tooltip("Output image width")]
        public int outputWidth = 5;
        [Tooltip("Output image height")]
        public int outputHeight = 5;
        [Tooltip("Between 1 and 8, any other value will be tweaked")]
        public int simmetry = 8;
        [Tooltip("")]
        public bool periodic = false;
        [Tooltip("")]
        public String savedName = "";

        private UnityEvent<Tilemap> outputEditionEvent;
        internal void FillTilemap()
        {
            Debug.Log("Potato");
            if (wfc.Fill(seed, maxIterations > 0 ? maxIterations : int.MaxValue))
                wfc.Save("owo");
            else Debug.Log("Faileada");

        }

        [Tooltip("Funciona Entropy y Scanline")]
        public Model.Heuristic heuristicType;
        [Tooltip("Funciona Entropy y Scanline")]
        public int seed;
        OverModel wfc;

        // Start is called before the first frame update
        void Start()
        {
            CreateWFC();

            CreateTilemap();
            SaveTilemap();
        }
        public void CreateWFC()
        {
            if (simmetry < 1) simmetry = 1;
            if (simmetry > 8) simmetry = 8;

            if (inputGrid && outputGrid)
            {
                outputGrid.ClearAllTiles();
                wfc = new OverModel(inputGrid, outputGrid, patternSize, this.outputWidth, this.outputHeight, this.periodic, this.simmetry, heuristicType);
            }
            wfc.Init();
        }
        public void CreateTilemap()
        {
            if (wfc.Run(seed, maxIterations > 0 ? maxIterations : int.MaxValue))
                wfc.Save("owo");
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
                AssetDatabase.CreateFolder("Assets", "SavedTilemaps");
                saveTilemapPrefab(dir);
            }

        }

        internal void GenerateSeed()
        {
            System.Random s = new System.Random();
            seed = s.Next();

        }
        private void saveTilemapPrefab(String s)
        {
            GameObject prefab = PrefabUtility.CreatePrefab(s, outputGrid.gameObject, ReplacePrefabOptions.ReplaceNameBased);
            Debug.Log("Saved as " + s);
        }



    }



}
