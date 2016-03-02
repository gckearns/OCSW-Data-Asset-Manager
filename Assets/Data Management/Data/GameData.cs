using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;
using System.Text;

[Serializable]
public class GameData : DataObject {
    
    public GameDataCategory category = GameDataCategory.None;
    public GameDetailsDictionary attributes = new GameDetailsDictionary (); //probably need to make this a list?

    public override void OnGUI () {
        base.OnGUI ();
        category = (GameDataCategory) EditorGUILayout.EnumPopup ("Category", category);
        if (attributes.dataType == GameDataType.None) {
            Debug.Log ("Attributes were null, creating details dictionary...");
//            attributes = new GameDetailsDictionary (dataType);
            attributes.dataType = dataType;
        }
        attributes.OnGUI ();
    }

    public override bool OnDBDataRemoved (Type t, int index)
    {
        Debug.Log (t.ToString() + " database removed index " + index);
        if (t == typeof (GameAttribute)) {
            attributes.OnAttributeRemoval (index);
        }
        return true;
    }
}
