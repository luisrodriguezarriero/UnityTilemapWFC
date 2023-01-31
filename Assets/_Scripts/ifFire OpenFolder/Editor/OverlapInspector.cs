using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace fire
{
    [CustomEditor(typeof(OverlapEditor))]

    public class LuysoWFCOverlapInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            OverlapEditor myScript = (OverlapEditor)target;
            if (GUILayout.Button("Create tilemap"))
            {
                myScript.CreateWFC();
                myScript.CreateTilemap();
            }
            if (GUILayout.Button("Save tilemap"))
            {
                myScript.SaveTilemap();
            }
            if (GUILayout.Button("Init Tilemap"))
            {
                myScript.ClearTilemap();
            }
            if (GUILayout.Button("Fill Tilemap"))
            {
                myScript.FillTilemap();
            }
        }
    }
}
