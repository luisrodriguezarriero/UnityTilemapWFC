using UnityEngine;
using UnityEditor;


namespace Snake.Editor{

    [CustomEditor(typeof(UI.Controller))]
    [CanEditMultipleObjects]
    public class LookAtPointEditor : UnityEditor.Editor
    {
        SerializedProperty lookAtPoint;
        
        void OnEnable()
        {
            lookAtPoint = serializedObject.FindProperty("lookAtPoint");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(lookAtPoint);
            serializedObject.ApplyModifiedProperties();
        }
    }
}