using UnityEngine;
using System.Collections;
using UnityEditor;


[System.Serializable]
public class ListenerDatabase : ScriptableObject {

    public delegate bool DataChangeListener (System.Type dataObjectType, int index);

    public event DataChangeListener DataRemovedListener;
    public event DataChangeListener DataAddedListener;

    public DataChangeListener dataRemovedListener { get { return DataRemovedListener; } }
    public DataChangeListener dataAddedListener { get { return DataAddedListener; } }

    private static string assetsString = "Assets";
    private static string resourceString = "Resources";
    private static string resourcePath = "Assets/Resources";
    private static string DbString = "ListenerDatabase";
    private static string DbPath {get {return resourcePath + "/" + DbString;}}

    [SerializeField]
    private static ListenerDatabase _current = null;
    public static ListenerDatabase current { 
        get {
            if (_current == null) {
                return Init ();
            } else {
                return _current; 
            }
        }
        set {
            _current = value;
        }
    }

    public bool OnDBDataRemoved (System.Type t, int index) {
        return DataRemovedListener (t, index);
    }

    public bool OnDBDataAdded (System.Type t, int index) {
        return DataAddedListener (t, index);
    }

    private static ListenerDatabase Init () {
        ValidateFolders ();
        _current = Resources.Load <ListenerDatabase> (DbString + "/" + DbString);
        if (_current == null) {
            _current = ScriptableObject.CreateInstance<ListenerDatabase>();
            AssetDatabase.CreateAsset (_current, DbPath + "/" + DbString + ".asset");
            AssetDatabase.SaveAssets ();
        }
        Debug.Log ("Assets validated and Saved. Database list initialized.");
        return _current;
    }

    private static void ValidateFolders() {
        //        Debug.Log ("Validating directory structure...");
        if (!AssetDatabase.IsValidFolder (resourcePath)) {
            AssetDatabase.CreateFolder (assetsString, resourceString);
        }
        if (!AssetDatabase.IsValidFolder (DbPath)) {
            AssetDatabase.CreateFolder (resourcePath, DbString);
        }
    }
}
