using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

[Serializable]
public class AttributeDatabase : ObjectDatabase <GameAttribute> {
    public override GameAttribute GetDefaultObject ()
    {
        GameAttribute ga = ScriptableObject.CreateInstance <GameAttribute> ();
        ga.typeEnum = GameAttributeType.Int;
        return ga;
    }
}
