using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

[Serializable]
public class ItemAttributeList : List<AttributeValue> {
    public void OnGUI () {
        for (int i = 0; i < Count; i++) {
            this [i].OnGUI ();
        }
    }
}
