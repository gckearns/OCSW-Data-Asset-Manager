using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

public abstract class DatabaseManager<TDatabase,TObject> 
    where TDatabase : ObjectDatabase<TObject> 
    where TObject : DataObject {

    private static List<TDatabase> databases = null;
    //    private GameDataDictionary dataDictionary = new GameDataDictionary();
    private static string assetsString = "Assets";
    private static string resourceString = "Resources";
    private static string resourcePath = "Assets/Resources";
    private static string DbString {
        get {
            return typeof(TDatabase).Name;
        }
    }
//    private static string dbPath = "Assets/Resources/GameDatabases"; // make this unique for each manager
    private static string DbPath {get {return resourcePath + "/" + DbString;}}

    public static List<TDatabase> Databases { 
        get { 
            if (databases != null) {
                Debug.Log ("Dictionary already initialized.");
                return databases; 
            } else {
                return InitializeDictionary ();
            }
        }
        set {
            databases = value;
        }
    }

    private static List<TDatabase> InitializeDictionary () {
        //        Debug.Log ("Initializing Dictionary");
        ValidateFolders ();
        databases = new List<TDatabase> ();
        foreach (GameDataType gameDataType in GameDataUtilities.allDataTypes) {
            TDatabase diskDatabase = Resources.Load <TDatabase> (DbString + "/" + gameDataType.ToString ());
            if (diskDatabase == null) {
                //                Debug.Log ("Data type does not exist on disk.  Creating...");
                TDatabase gdb = ScriptableObject.CreateInstance<TDatabase>();
                gdb.dataType = gameDataType;
                AssetDatabase.CreateAsset (gdb, DbPath + "/" + gameDataType.ToString () + ".asset");
                //                dataDictionary.Add (gameDataType, gdb);
                databases.Add (gdb);
            } else {
                //                dataDictionary.Add (gameDataType, diskDatabase);
                databases.Add (diskDatabase);
                //                    Debug.Log ("Integrated data with dictionary.");
            }
        }
        AssetDatabase.SaveAssets ();
        //        dataDictionary.isInitialized = true;
        Debug.Log ("Assets validated and Saved. Done initializing database list.");
        return databases;
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