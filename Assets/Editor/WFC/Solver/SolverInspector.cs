
using UnityEngine;
#if  UNITY_EDITOR
using UnityEditor;
#endif

namespace WFC
{
#if  UNITY_EDITOR

    [CustomEditor(typeof(Solver))] 
    public class SolverInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Solver myScript = (Solver)target;

            if (GUILayout.Button("import from Json"))
            {
                myScript.importFromJson();
            }

        }
    }
#endif
}

