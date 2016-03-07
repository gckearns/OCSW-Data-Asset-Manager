using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class GameItemDatabase : ObjectDatabase <GameItem>  {

    public override GameItem GetDefaultObject ()
    {
        GameItem gd = ScriptableObject.CreateInstance <GameItem> ();
        gd.dataType = dataType;
        gd.category = GameDataCategory.Services;
        gd.dataObjectName = string.Format ("Default {0}", dataType.ToString ());
        gd.dataObjectID = string.Format ("default{0}", dataType.ToString ());
        gd.name = gd.dataObjectID;
        AttributeDatabase adb = GameAttributeManager.Databases [GameDataUtilities.allDataTypes.IndexOf(dataType)];
        foreach (var attribute in adb.myData) {
            AttributeValue av = ScriptableObject.CreateInstance <AttributeValue> ();
            av.gAttribute = attribute;
            gd.attributes.Add (av);
        }
        Debug.Log ("Created default" + gd.ToString () + " with " + gd.attributes.Count + " attributes");
        return gd;
    }

    public override GameItem SetObjectDefaults (GameItem gameItem)
    {
        ItemAttributeList ial = new ItemAttributeList ();
//        ial.Capacity = defaultDataObject.attributes.Count;
        for (int i = 0; i < defaultDataObject.attributes.Count; i++) {
            ial.Insert(i, (AttributeValue) AttributeValue.Instantiate(defaultDataObject.attributes [i]));
            ial [i].gAttribute = defaultDataObject.attributes [i].gAttribute;
        }
        gameItem.attributes = ial;
        Debug.Log ("Set new " + gameItem.ToString () + " with " + gameItem.attributes.Count + " attributes");
        return gameItem;
    }
}
