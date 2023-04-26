using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if  UNITY_EDITOR
using UnityEditor;
#endif

namespace WFC_Unity_Luyso
{
#if  UNITY_EDITOR
    [CustomEditor(typeof(OverlapEditor))]

    public class LuysoWFCOverlapInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            OverlapEditor myScript = (OverlapEditor)target;
            if (myScript.inputGrid != null)
            {
                if (GUILayout.Button("Create Map"))
                {
                    myScript.CreateWFC();
                    myScript.GenerateSeed();
                    myScript.SolveWFC();
                }
                
                if (GUILayout.Button("Create Overlapping Model"))
                {
                    myScript.CreateWFC();
                }

                if (myScript.model != null)
                {
                    if (GUILayout.Button("Solve Model"))
                    {
                        myScript.GenerateSeed();
                        myScript.SolveWFC();
                    }

                    if (GUILayout.Button("Solve Model from seed"))
                    {
                        myScript.SolveWFC();
                    }
                }

                if (GUILayout.Button("Save Model"))
                {
                    myScript.SaveModel();
                }

                if (GUILayout.Button("Save Map"))
                {
                    myScript.SaveTilemap();
                }
            }
        }
    }
#endif
}
