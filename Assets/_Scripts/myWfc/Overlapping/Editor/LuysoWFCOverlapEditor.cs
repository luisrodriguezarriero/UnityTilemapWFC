using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
namespace LuysoWFC
{
    public class LuysoWFCOverlapEditor : MonoBehaviour
    {
    public GameObject inputGrid;
    public GameObject outputGrid;
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

        internal void FillTilemap()
        {
            throw new NotImplementedException();
        }

        [Tooltip("Funciona Entropy y Scanline")]
    public HeuristicType heuristicType;
        [Tooltip("Funciona Entropy y Scanline")]
    public int seed;
        LuysoOverlapModel wfc;

    // Start is called before the first frame update
    void Start()
    {
        CreateWFC();
        CreateTilemap();
        SaveTilemap();
    }
        public void ClearTilemap()
        {
            Tilemap[] output = outputGrid.GetComponentsInChildren<Tilemap>();
            if (output.Length > 0) output[0].ClearAllTiles();
            if (wfc == null) CreateWFC();
        }
        public void CreateWFC()
    {
        Tilemap[] input = inputGrid.GetComponentsInChildren<Tilemap>();
        Tilemap[] output = outputGrid.GetComponentsInChildren<Tilemap>();

            if (simmetry < 1) simmetry = 1;
            if (simmetry > 8) simmetry = 8;
        if (input.Length > 0 && output.Length > 0)
        {
            output[0].ClearAllTiles();
            wfc = new LuysoOverlapModel(input[0], output[0], patternSize, this.outputWidth, this.outputHeight, this.periodic, this.simmetry, heuristicType);
        }

        }
    public void CreateTilemap()
    {
            GenerateSeed();
            CreateWFC();
            if (wfc.Run(seed, maxIterations))
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
        } catch (ArgumentException e)
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
            GameObject prefab = PrefabUtility.CreatePrefab(s, (GameObject)outputGrid, ReplacePrefabOptions.ReplaceNameBased);
            Debug.Log("Saved as " + s);
        }

    }
}
