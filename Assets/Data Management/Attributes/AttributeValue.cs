using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AttributeValue : ScriptableObject {

    [SerializeField]
    public ItemAttribute gAttribute;

    public int intValue {
        get {
            return _intValue;
        }
        set {
            _intValue = value;
        }
    }
    
    public float floatValue {
        get { 
            return _floatValue; 
        }
        set {
            _floatValue = value;
        }
    }

    [SerializeField]
    private int _intValue = 0;
    [SerializeField]
    private float _floatValue = 0;

    public void OnGUI () {
        gAttribute.OnGUI (this);
    }
}
