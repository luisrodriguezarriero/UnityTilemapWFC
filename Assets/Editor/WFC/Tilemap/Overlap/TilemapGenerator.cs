using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WFC.Tilemap.Overlap
{
    public class TilemapGenerator : WFC.Unity.MapGenerator
    {
        [Header("Tilemap Objects")]
        public UnityEngine.Tilemaps.Tilemap inputGrid;
        public UnityEngine.Tilemaps.Tilemap outputGrid;

        protected ModelCreator wfc;

        public override void CreateModel()
        {            
            wfc = new ModelCreator(inputGrid, N,
                periodicInput, simmetry, false);
            model = wfc.model;
            
        }
        public override void Solve()
        {
            GenerateSeed();
            ClearOutput();
            var solver = initSolver(MX, MY, periodicOutput);
            if (solver.Run(seed, maxIterations > 0 ? maxIterations : int.MaxValue))
                wfc.Save(outputGrid, solver.Result, MX, MY);
            else Debug.Log("Fail");
        }

        protected virtual void ClearOutput()
        {
            if (outputGrid)
            {
                outputGrid.ClearAllTiles();
            }
        }

        public override void CreateMap()
        {
            if(!inputGrid || !outputGrid) 
            {
                Debug.LogError("INPUT AND OUTPUT MUST NOT BE NULL");
                return;
            }
            
            inputGrid.CompressBounds();
            outputGrid.CompressBounds();

            CreateModel();
        }
    }

        #if  UNITY_EDITOR

    [CustomEditor(typeof(TilemapGenerator))]

    public class WfcInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TilemapGenerator myScript = (TilemapGenerator)target;
            if (myScript.inputGrid)
            {
                if(myScript.modelExists){
                    if (GUILayout.Button("Solve from model"))
                    {
                        myScript.Solve();
                    }
                    if (GUILayout.Button("Reset Model"))
                    {
                        myScript.CreateModel();
                    }
                }
                else
                
                if (GUILayout.Button("CreateModel"))
                {
                    myScript.CreateModel();
                }

                if (GUILayout.Button("New Object & Map"))
                {
                    myScript.CreateMap();
                }

            }


        }
    }
    #endif
}
