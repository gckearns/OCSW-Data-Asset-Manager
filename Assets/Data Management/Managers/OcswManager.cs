using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class OcswManager {
    [SerializeField]
    private static OcswDatabase database = null;
    //    private GameDataDictionary dataDictionary = new GameDataDictionary();
    private static string assetsString = "Assets";
    private static string resourceString = "Resources";
    private static string resourcePath = "Assets/Resources";
    private static string DbString = "OCSW Database";
    private static string DbPath {get {return resourcePath + "/" + DbString;}}

    public static OcswDatabase Database { 
        get { 
            if (database == null) {
                return Init ();
            } else {
                return database; 
            }
        }
        set {
            database = value;
        }
    }

    private static OcswDatabase Init () {
        ValidateFolders ();
        database = Resources.Load <OcswDatabase> (DbString + "/" + DbString);
        if (database == null) {
            database = ScriptableObject.CreateInstance<OcswDatabase>();
            AssetDatabase.CreateAsset (database, DbPath + "/" + DbString + ".asset");
            AssetDatabase.SaveAssets ();
        }
        Debug.Log ("OcswDatabase initialized.");
        return database;
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