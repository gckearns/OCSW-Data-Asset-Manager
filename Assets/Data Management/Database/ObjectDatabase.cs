using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;

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
    private TObject _defaultDataObject = null;
    protected TObject defaultDataObject {
        get {
            if (_defaultDataObject == null) {
                _defaultDataObject = GetDefaultObject ();
                FinalizeObject (_defaultDataObject);
            }
            return _defaultDataObject;
        }
        set {
            _defaultDataObject = value;
        }
    }

    private OcswDatabase _myDatabase = null;
    private OcswDatabase myDatabase {
        get {
            if (_myDatabase == null) {
                _myDatabase = OcswManager.Database;
            }
            return _myDatabase;
        }
    }

    public void AddDataObject (string dataName, string dataID) {
//        TObject gd = ScriptableObject.CreateInstance<TObject> ();
        TObject obj = SetObjectDefaults (ScriptableObject.Instantiate <TObject> (defaultDataObject));
        obj.dataObjectName = dataName;
        obj.name = dataID;
        obj.dataObjectID = dataID;
        obj.dataType = dataType;
        myIDs.Add (dataID);
        myData.Insert (myIDs.IndexOf(dataID), obj);
        int i = myData.IndexOf (obj);
        FinalizeObject (obj);
        if (myDatabase.dataAddedListener != null) {
            myDatabase.OnDBDataAdded (typeof (TObject), i);
        }
    }

    public void RemoveData (string dataID) {
        int i = myIDs.IndexOf (dataID);
        TObject obj = myData [i];
        if (obj.dataObjectID != dataID) {
            Debug.Log ("DELETION ID INDEX DOESN'T MATCH DELETION CANDIDATE"); //can probably delete this if block
        }
        myDatabase.DataAddedListener -= obj.OnDBDataAdded;
        myDatabase.DataRemovedListener -= obj.OnDBDataRemoved;
        if (myDatabase.dataRemovedListener != null) {
            myDatabase.OnDBDataRemoved (typeof (TObject), i);
        }
        DestroyImmediate (obj, true);
        myIDs.RemoveAt (i);
        myData.RemoveAt (i);
        AssetDatabase.SaveAssets (); // I could do something else with this to allow UNDO actions

    }

    public void ResetDatabase () {
        while (myData.Count > 0) {
            RemoveData (myData[myData.Count - 1].dataObjectID);
        }
        myData.Clear();
        myData.TrimExcess();
        myIDs.Clear();
        myIDs.TrimExcess ();
        DestroyImmediate (defaultDataObject, true);
        AssetDatabase.SaveAssets ();
        defaultDataObject = GetDefaultObject ();
        FinalizeObject (defaultDataObject);
        Debug.Log (dataType.ToString() + " database reset.");
    }

    private void FinalizeObject (TObject dataObject) {
        myDatabase.DataAddedListener += dataObject.OnDBDataAdded;
        myDatabase.DataRemovedListener += dataObject.OnDBDataRemoved;
        AssetDatabase.AddObjectToAsset (dataObject, this);
        AssetDatabase.SaveAssets ();
    }

    public abstract TObject GetDefaultObject ();

    public abstract TObject SetObjectDefaults (TObject item);
}
