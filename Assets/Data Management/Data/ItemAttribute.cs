using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

[Serializable]
//[HideInInspector]
public class ItemAttribute : DataObject {
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
            UpdateAttributeType ();
        }
        if (typeEnum == GameAttributeType.IntSlider) {
            sliderMinimum = EditorGUILayout.IntField ("Slider Min.", sliderMinimum);
            sliderMaximum = EditorGUILayout.IntField ("Slider Max.", sliderMaximum);
        }
    }

    public void OnGUI (AttributeValue attributeVal) {
        if (updateValue == null) {
            UpdateAttributeType (); // maybe can get rid of this if block
        }
        updateValue (attributeVal);
    }

    public void SetAttributeType (GameAttributeType typeEnum) {
        this.typeEnum = typeEnum;
        UpdateAttributeType ();
    }

    public void UpdateAttributeType () {
        switch (typeEnum) {
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
            throw (new MissingFieldException("This attribute's 'updateValue' is null. Can't set GUI delegate."));
//            break;
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

    public override bool OnDBDataAdded (Type t, int index)
    {
        return true;
    }

    public override bool OnDBDataRemoved (Type t, int index)
    {
        return true;
    }
}
