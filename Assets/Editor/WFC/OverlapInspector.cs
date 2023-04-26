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
            if (GUILayout.Button("New tilemap"))
            {
                myScript.GenerateSeed();
                myScript.CreateWFC();
                myScript.CreateTilemap();
            }
            if (GUILayout.Button("Create Empty Tilemap"))
            {
                myScript.CreateWFC();
            }
            if (GUILayout.Button("Fill Tilemap"))
            {
                myScript.FillTilemap();
            }
            if (GUILayout.Button("New Tilemap From Seed"))
            {
                myScript.CreateWFC();
                myScript.CreateTilemap();
            }
            if (GUILayout.Button("Save tilemap"))
            {
                myScript.SaveTilemap();
            }
        }
    }
}
