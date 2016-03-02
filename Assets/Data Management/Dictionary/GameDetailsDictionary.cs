using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

[Serializable]
public class GameDetailsDictionary : List<AttributeValue> {
    
    public GameDataType dataType = GameDataType.None;
    public List <GameAttribute> attributes = new List <GameAttribute>();
    public List <AttributeDatabase> databases = null;

    public GameDetailsDictionary () {
        Debug.Log ("Details object constructor");
        if (databases == null) {
            databases = GameAttributeManager.Databases;
        }    
    }

    public void OnGUI () {
        AttributeDatabase adb = databases[GameDataUtilities.allDataTypes.IndexOf (dataType)];
        for (int i = 0; i < Count; i++) {
            if (!adb.myData.Contains(attributes[i])) {
                attributes.RemoveAt (i);
                RemoveAt (i);
            }
        }
        foreach (GameAttribute attribute in adb.myData) {
            if (!attributes.Contains(attribute)) {
                AttributeValue newValue = ScriptableObject.CreateInstance <AttributeValue> ();
                newValue.gAttribute = attribute;
                this.Add (newValue);
                attributes.Add (attribute);
            }
        }
        for (int i = 0; i < Count; i++) {
            this [i].OnGUI ();
        }
    }

    public bool OnAttributeRemoval (int index) {
        Debug.Log ("Updated " + dataType.ToString () + " details");
        attributes.RemoveAt (index);
        this.RemoveAt (index);
        return true;
    }
}
