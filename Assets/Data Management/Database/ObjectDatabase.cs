using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;

public delegate bool DataChangeListener (Type dataObjectType, int index);

public class MyAssetProcessor : AssetModificationProcessor
{
    static string[] OnWillSaveAssets (string[] paths) {
        Debug.Log ("OnWillSaveAssets");
        return paths;
    }

    static AssetDeleteResult OnWillDeleteAsset (string path, RemoveAssetOptions option) {
        Debug.Log ("OnWillDeleteAssets");
        Debug.Log (path);
        Debug.Log (option.ToString());
        return AssetDeleteResult.FailedDelete;
    }

}

[Serializable]
public abstract class ObjectDatabase <TObject> : ScriptableObject
    where TObject : DataObject {

    [SerializeField]
    public GameDataType dataType = GameDataType.None;
    [SerializeField]
    public List<string> myIDs = new List<string>();
    [SerializeField]
    public List<TObject> myData = new List<TObject>();
    [SerializeField]
    protected TObject _defaultDataObject = null;

    public TObject defaultDataObject {
        get {
            if (_defaultDataObject == null) {
                _defaultDataObject = GetDefaultObject ();
            }
            return _defaultDataObject;
        }
        set {
            _defaultDataObject = value;
        }
    }

    void OnEnable () {
        //        hideFlags = HideFlags.HideInInspector;

    }

    public void AddDataObject (string dataID) {
        TObject gd = ScriptableObject.CreateInstance<TObject> ();
//        TObject gd = ScriptableObject.Instantiate <TObject> (_defaultDataObject);
        gd.dataType = dataType;
        gd.dataObjectID = dataID;
        myIDs.Add (dataID);
        myData.Add (gd);
        gd.name = dataID;
        GameDataUtilities.DataRemovedListener += gd.OnDBDataRemoved;
        AssetDatabase.AddObjectToAsset (gd, this);
        AssetDatabase.SaveAssets ();
        Debug.Log ("Saved GameData Assets");
    }

    public void RemoveData (string dataID) {
        int i = myIDs.IndexOf (dataID);
        TObject gd = myData [i];
        if (GameDataUtilities.dataRemovedListener != null) {
            GameDataUtilities.OnDBDataRemoved (typeof (TObject), i);
        }
        DestroyImmediate (gd, true);
        myIDs.RemoveAt (i);
        myData.RemoveAt (i);
        AssetDatabase.SaveAssets (); // I could do something else with this to allow UNDO actions
    }

    public void ResetDatabase () {
        foreach (TObject dataObject in myData) {
            DestroyImmediate (dataObject, true);
        }
        myData = new List<TObject> ();
        myIDs = new List<string> ();
        AssetDatabase.SaveAssets ();
        Debug.Log ("Database has been reset.");
    }

    public abstract TObject GetDefaultObject ();
}
