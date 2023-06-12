using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
#if  UNITY_EDITOR
using UnityEditor;
#endif

namespace WFC
{
    public class SnakeMapEditor : MonoBehaviour
    {
        
        public int maxIterations = 9999;
        
        public List<Tilemap> input;
        
        [Tooltip("Recommended values between 2 and 4")]
        public int[] tileSize;
        
        public Tilemap output;
        private List<SnakeMapCreator> wfc = new List<SnakeMapCreator>();
        public int symmetry = 1;
        public bool periodicInput, periodic;
        public bool ground;
        public  Heuristic heuristic;
        private int seed;
        [SerializeField]
        private  int outputWidth, outputHeight;
        
        public IEnumerator CreateMaps()
        {
            checkSimmetry();
            wfc = new List<SnakeMapCreator>();
            for(var i=0; i<input.Count; i++)
            {
                CreateMap(i);
                Solve(i);
            }
            yield return null;
        }

        public void CreateMap(int i)
        {
            checkSimmetry();
            input[i].CompressBounds();
            output.CompressBounds();
            var aux = new SnakeMapCreator(input[i], tileSize[i], periodicInput, symmetry, ground);
            if (wfc.Count > i) wfc.RemoveAt(i);
            wfc.Insert(i, aux);
            Solve(i);
        }

        public void Solve(int i)
        {
            GenerateSeed();
           (outputWidth, outputHeight) = getRealOutputBounds(output);
            var solver = new WfcSolver(wfc[i].GetModel(), outputWidth, outputHeight, this.heuristic, this.periodic);
            if (solver.Run(seed, maxIterations > 0 ? maxIterations : int.MaxValue))
                wfc[i].Save(output, solver.Result);
            else Debug.Log("Fail");
            
        }

        private (int outputWidth, int outputHeight) getRealOutputBounds(Tilemap tilemap)
        {
            var bounds = tilemap.cellBounds;
            bounds.SetMinMax(new Vector3Int(bounds.xMin+1, bounds.yMin+1, 0), new Vector3Int(bounds.xMax-1, bounds.yMax-1, 0));
            return (bounds.size.x, bounds.size.y);
        }

        internal void ClearOutput(Tilemap output)
        {
            var bounds = new BoundsInt();
            bounds.SetMinMax(new Vector3Int(output.cellBounds.xMin+2, output.cellBounds.yMin+2, 0), new Vector3Int(output.cellBounds.xMax-2, output.cellBounds.yMax-2, 0));
            
            for(var i = bounds.xMin; i<bounds.xMax; i++)
            {
                for(var j = bounds.yMin; j<bounds.yMax; j++)
                {
                    output.SetTile(new Vector3Int(i, j, 0), null);
                }
            }
        }
        public bool hasInput()
        {
            return this.input!=null && this.input.Count>0;
        }

        internal void GenerateSeed()
        {
            var s = new System.Random();
            seed = s.Next();
        }

        protected void checkSimmetry()
        {
            if (symmetry < 1) symmetry = 1;
            if (symmetry > 8) symmetry = 8;
        }

        public TilemapWfcModel getModelByIndex(int i)
        {
            if (wfc.Count >= i)
                return wfc[i].GetModel();
            return null;
        }
    } 
    
    #if  UNITY_EDITOR
    [CustomEditor(typeof(SnakeMapEditor))]
        public class SnakeMapInspector : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
                
                SnakeMapEditor myScript = (SnakeMapEditor)target;
                for (int i = 0; i < myScript.input.Count; i++)
                {
                    if (GUILayout.Button($"Create Map from {myScript.input[i].name}"))
                    {
                        myScript.CreateMap(i);
                    }
                }
                if(myScript.hasInput()){
                    if (GUILayout.Button("Create Map"))
                    {
                        myScript.CreateMaps();
                    }
                }
            }
        }
    #endif
}

