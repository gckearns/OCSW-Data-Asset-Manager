using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;
using System.Text;

[Serializable]
public abstract class DataObject : ScriptableObject {

    public GameDataType dataType = GameDataType.None;
    public string dataObjectName = "New Object";
    public string dataObjectID = "NewObject";
    public string description = "No description.";
    public Sprite objectIcon;

    public virtual void OnGUI() {
        dataObjectName = EditorGUILayout.TextField ("Name", dataObjectName);
        EditorGUILayout.LabelField ("ID", dataObjectID);
    }

    public abstract bool OnDBDataRemoved (Type t, int index);
}
