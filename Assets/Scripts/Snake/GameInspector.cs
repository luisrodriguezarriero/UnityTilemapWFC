using UnityEngine;
using UnityEditor;

namespace Snake
{
#if UNITY_EDITOR

    [CustomEditor(typeof(GameManager))]

    public class GameInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GameManager myScript = (GameManager)target;
            if (GUILayout.Button("Setup"))
                {
                    myScript.TestSetup();
                }
        }
    }

#endif
}