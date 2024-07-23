#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Snake.Setup
{

    [CustomEditor(typeof(TilemapGenerator))]
    public class WfcInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TilemapGenerator myScript = (TilemapGenerator)target;
            if (myScript.inputGrid)
            {
                timeTestUtils.Testing=true;
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
}

    #endif


