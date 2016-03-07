using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (OcswDatabase))]
public class OcswEditor : Editor {

    public override void OnInspectorGUI ()
    {
        base.OnInspectorGUI ();
        OcswDatabase odb = target as OcswDatabase;
        EditorGUILayout.LabelField ("AddDataListener(s)", odb.dataAddedListener != null ? odb.dataAddedListener.GetInvocationList ().Length.ToString() : "null");
        EditorGUILayout.LabelField ("RemoveDataListener(s)", odb.dataRemovedListener != null ? odb.dataRemovedListener.GetInvocationList ().Length.ToString () : "null");
        if (GUILayout.Button("RESET DATABASE")) {
            odb.ResetDatabase ();
        }
    }
}