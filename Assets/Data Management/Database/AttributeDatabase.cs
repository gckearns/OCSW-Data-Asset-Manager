using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

[Serializable]
public class AttributeDatabase : ObjectDatabase <ItemAttribute> {
    
    public override ItemAttribute GetDefaultObject ()
    {
        ItemAttribute att = ScriptableObject.CreateInstance <ItemAttribute> ();
        att.dataObjectName = string.Format ("Default {0}", dataType.ToString ());
        att.dataObjectID = string.Format ("default{0}", dataType.ToString ());
        att.name = att.dataObjectID;
        att.dataType = dataType;
        att.SetAttributeType (GameAttributeType.Int);
        return att;
    }

    public override ItemAttribute SetObjectDefaults (ItemAttribute item) {
        return item;
    }
}
