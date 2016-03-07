using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

[Serializable]
public class GameItemManager : DatabaseManager<GameItemDatabase,GameItem> {
}

[Serializable]
public class GameAttributeManager : DatabaseManager<AttributeDatabase,ItemAttribute> {
}

public abstract class DatabaseManager<TDatabase,TObject> 
    where TDatabase : ObjectDatabase<TObject> 
    where TObject : DataObject {
    [SerializeField]
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
    private static string DbPath {get {return resourcePath + "/" + DbString;}}

    public static List<TDatabase> Databases { 
        get { 
            if (databases == null || databases.Contains(null)) {
                return InitializeDictionary ();
            } else {
                return databases; 
            }
        }
        set {
            databases = value;
        }
    }

    private static List<TDatabase> InitializeDictionary () {
        ValidateFolders ();
        databases = new List<TDatabase> ();
        foreach (GameDataType gameDataType in GameDataUtilities.allDataTypes) {
            TDatabase diskDatabase = Resources.Load <TDatabase> (DbString + "/" + gameDataType.ToString ());
            if (diskDatabase == null) {
                TDatabase gdb = ScriptableObject.CreateInstance<TDatabase>();
                AssetDatabase.CreateAsset (gdb, DbPath + "/" + gameDataType.ToString () + ".asset");
                gdb.dataType = gameDataType;
                databases.Add (gdb);
            } else {
                databases.Add (diskDatabase);
            }
        }
        AssetDatabase.SaveAssets ();
        Debug.Log ("Assets validated and Saved. Database list initialized.");
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