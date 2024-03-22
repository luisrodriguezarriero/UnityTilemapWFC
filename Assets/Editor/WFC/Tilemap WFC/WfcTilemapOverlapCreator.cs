using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WFC.TiledWFC
{
    public class WfcTilemapOverlapCreator : WfcCreator
    {
        [Header("Tilemap Objects")]
        public Tilemap inputGrid;
        public Tilemap outputGrid;
        public Tilemap floorGrid;

        [Header("Layers")]
        public List<TileBase> floorTiles;
        public List<TileBase> wallTiles;

        protected OverlappingTilemapWfcModelCreator wfc;

        public override void CreateModel()
        {            
            wfc = new OverlappingTilemapWfcModelCreator(inputGrid, N,
                periodicInput, simmetry, false);
            model = wfc.model;
            
        }
        public override void Solve()
        {
            GenerateSeed();
            ClearOutput();
            var solver = initSolver(MX, MY, periodicOutput);
            if (solver.Run(seed, maxIterations > 0 ? maxIterations : int.MaxValue))
                wfc.Save(outputGrid, solver.Result, MX, MY, floorGrid);
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

    [CustomEditor(typeof(WfcTilemapOverlapCreator))]

    public class WfcInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            WfcTilemapOverlapCreator myScript = (WfcTilemapOverlapCreator)target;
            if (myScript.inputGrid)
            {
                if(myScript.modelExists){
                    if (GUILayout.Button("Solve from model"))
                    {
                        myScript.Solve();
                    }
                }
                
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
