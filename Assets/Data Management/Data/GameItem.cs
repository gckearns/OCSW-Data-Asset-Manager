using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;
using System.Text;

[Serializable]
public class GameItem : DataObject {

    // need to make this unique to the specific GameItem database
    // Commoddities aren't categorized by service/industry/housing
    [SerializeField]
    public GameDataCategory category = GameDataCategory.None; 
    [SerializeField]
    public ItemAttributeList attributes = new ItemAttributeList ();
//    public List <AttributeDatabase> databases = null;

//    public void OnEnable () {
//        databases = GameAttributeManager.Databases;
//        attributes = new ItemAttributeList (dataType);
//        Debug.Log ("GameItem.OnEnable: Attributes were null, creating details dictionary...");
//    }

    public override void OnGUI () {
        base.OnGUI ();
        category = (GameDataCategory) EditorGUILayout.EnumPopup ("Category", category);
        attributes.OnGUI ();
    }

    public override string ToString ()
    {
        return string.Format ("[GameItem]:{0} Atts", attributes.Count);
    }

    public override bool OnDBDataAdded (Type t, int index)
    {
        Debug.Log (t.ToString() + " database added index " + index);
        if (t == typeof (ItemAttribute)) {
            AttributeValue av = ScriptableObject.CreateInstance <AttributeValue> ();
            av.gAttribute = GameAttributeManager.Databases [GameDataUtilities.allDataTypes.IndexOf (dataType)].myData[index]; // this is causing problems
            attributes.Add (av);
        }
        return true;
    }

    public override bool OnDBDataRemoved (Type t, int index)
    {
        Debug.Log (t.ToString() + " database removed index " + index);
        if (t == typeof (ItemAttribute)) {
            attributes.RemoveAt (index);
        }
        return true;
    }
}
