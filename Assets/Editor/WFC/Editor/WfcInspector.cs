using UnityEditor;
using UnityEngine;

namespace WFC{

    #if  UNITY_EDITOR

    [CustomEditor(typeof(OverlapEditor))]

    public class WfcInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            OverlapEditor myScript = (OverlapEditor)target;
            if (myScript.hasInput())
            {
                if (GUILayout.Button("Create Map"))
                {
                    myScript.CreateMap();
                }
            }
        }
    }
    #endif
}