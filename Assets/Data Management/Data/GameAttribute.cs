using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

[Serializable]
//[HideInInspector]
public class GameAttribute : DataObject {
    public delegate void GUIDelegate(AttributeValue value);
    public GUIDelegate updateValue = null;
    public GameAttributeType typeEnum = GameAttributeType.Int;
    public int sliderMinimum = 0;
    public int sliderMaximum = 0;

    public override void OnGUI () {
        base.OnGUI ();
        EditorGUI.BeginChangeCheck ();
        typeEnum = (GameAttributeType) EditorGUILayout.EnumPopup ("Type", typeEnum);
        if (EditorGUI.EndChangeCheck()) {
            SetAttributeType (typeEnum);
        }
        if (typeEnum == GameAttributeType.IntSlider) {
            sliderMinimum = EditorGUILayout.IntField ("Slider Min.", sliderMinimum);
            sliderMaximum = EditorGUILayout.IntField ("Slider Max.", sliderMaximum);
        }
    }

    public void OnGUI (AttributeValue attributeVal) {
        if (updateValue == null) {
            SetAttributeType (typeEnum);
        }
        updateValue (attributeVal);
    }

    public override bool OnDBDataRemoved (Type t, int index)
    {
        Debug.Log (t.ToString() + " database removed index " + index);
        return true;
    }

    public void SetAttributeType (GameAttributeType attributeType) {
        switch (attributeType) {
        case GameAttributeType.Float:
            updateValue = GetFloat;
            Debug.Log (dataObjectName + " attribute set to float.");
            break;
        case GameAttributeType.Int:
            updateValue = GetInt;
            Debug.Log (dataObjectName + " attribute set to int.");
            break;
        case GameAttributeType.IntSlider:
            updateValue = GetIntSlider;
            Debug.Log (dataObjectName + " attribute set to int slider.");
            break;
        default:
            updateValue = null;
            break;
        }
    }

    public void GetInt (AttributeValue current) {
        current.intValue = EditorGUILayout.IntField (dataObjectName, current.intValue);
    }

    public void GetFloat (AttributeValue current) {
        current.floatValue = EditorGUILayout.FloatField (dataObjectName, current.floatValue);
    }

    public void GetIntSlider (AttributeValue current) {
        current.intValue = EditorGUILayout.IntSlider (dataObjectName, current.intValue, sliderMinimum, sliderMaximum);
    }
}
