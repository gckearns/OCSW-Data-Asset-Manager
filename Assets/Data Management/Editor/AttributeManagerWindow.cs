﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.UI;

public class AttributeManagerWindow : EditorWindow {

    private int selectedToolType;
    private int selectedToolSort;
    private int selectedListItem;
    private Vector2 scrollPositionLeft;
    private Vector2 scrollPositionRight;

//    public GameAttributeManager manager = new GameAttributeManager();
    //    private GameDataDictionary gameDictionary;
    private List<AttributeDatabase> databaseList = null;
    public AttributeDatabase selectedDatabase;
    private ItemAttribute selectedGameData;
    private List<ItemAttribute> gameDataList = new List<ItemAttribute>();

    [MenuItem ("Data Managers/Attribute Manager")]
    static void Init() {
        AttributeManagerWindow attWindow = (AttributeManagerWindow)EditorWindow.GetWindow (typeof(AttributeManagerWindow));
//        DataManagerWindow dataWindow = (DataManagerWindow)EditorWindow.GetWindow (typeof(DataManagerWindow));
//        dataWindow.Close ();
        attWindow.Show ();
    }

    void OnEnable () {
//        Debug.Log ("Attribute window enabled");
//        manager = new GameAttributeManager(); // do i really need this?
        if (databaseList == null) {
            databaseList = GameAttributeManager.Databases;    
        }
    }

    void OnGUI (){
        if (databaseList.Contains(null)) {
            databaseList = GameAttributeManager.Databases;
        }
        selectedToolType = GUILayout.Toolbar(selectedToolType, GetDataTypeNames());
        selectedDatabase = databaseList[selectedToolType];
        EditorGUILayout.BeginHorizontal (); { // Split list and data view
            EditorGUILayout.BeginVertical (); { // List of data
                selectedToolSort = GUILayout.Toolbar (selectedToolSort, new string[]{ "Name", "ID", "Type" });
                scrollPositionLeft = EditorGUILayout.BeginScrollView (scrollPositionLeft); {
                    EditorGUILayout.BeginHorizontal (); {
                        selectedGameData = null;
                        if (selectedDatabase.myData.Count > 0) {
                            if (selectedListItem >= selectedDatabase.myData.Count) {
                                selectedListItem = (selectedDatabase.myData.Count - 1);
                            }
                            if (selectedListItem < 0) {
                                selectedListItem = 0;
                            }
                            selectedListItem = GUILayout.SelectionGrid (selectedListItem, GetDataNames (), 1);
                            selectedListItem = GUILayout.SelectionGrid (selectedListItem, GetDataIDs (), 1);
                            selectedListItem = GUILayout.SelectionGrid (selectedListItem, GetCategoryNames(), 1);
                            selectedGameData = gameDataList [selectedListItem];
                        }
                    } EditorGUILayout.EndHorizontal ();
                } EditorGUILayout.EndScrollView ();
                if (GUILayout.Button("Add")){
                    if (!AddAttributeWindow.isOpen) {
//                        selectedDatabase.AddGameData (selectedDatabase.myData.Capacity.ToString());
                        AddAttributeWindow w = ScriptableObject.CreateInstance<AddAttributeWindow> ();
                        w.database = selectedDatabase;
                        w.parentWindow = this;
                        w.ShowUtility ();
                    }
                    //Refresh and select the new data
                }
            } EditorGUILayout.EndVertical (); 
            EditorGUILayout.BeginVertical (); { // data details
                if (selectedGameData != null) {
                    scrollPositionRight = EditorGUILayout.BeginScrollView (scrollPositionRight);
                    {
                        selectedGameData.OnGUI ();
                    }
                    EditorGUILayout.EndScrollView ();
                    if (GUILayout.Button ("Delete")) {
                        // Put a warning dialogue here
                        selectedDatabase.RemoveData (selectedGameData.dataObjectID);
                    }
                }
            } EditorGUILayout.EndVertical ();
        } EditorGUILayout.EndHorizontal ();
        Event e = Event.current;
        //        Debug.Log ("Event: " + e.type.ToString());
        if (e.type == EventType.MouseUp) {
            EditorGUIUtility.keyboardControl = 0;
        }
    }

    string[] GetDataTypeNames () {
        string[] gameDataTypeNames = new string[GameDataUtilities.allDataTypes.Count];
        for (int i = 0; i < GameDataUtilities.allDataTypes.Count; i++) {
            gameDataTypeNames[i] = GameDataUtilities.allDataTypes [i].ToString ();
        }
        return gameDataTypeNames;
    }

    string[] GetDataNames () {
        string[] gameDataNames = new string[selectedDatabase.myData.Count];
        gameDataList = selectedDatabase.myData;
        for (int i = 0; i < gameDataList.Count; i++) {
            gameDataNames [i] = gameDataList[i].dataObjectName;
        }
        return gameDataNames;
    }

    string[] GetDataIDs () {
        string[] gameDataIDs = new string[selectedDatabase.myData.Count];
        for (int i = 0; i < gameDataIDs.Length; i++) {
            gameDataIDs [i] = i.ToString ();
        }
        return gameDataIDs;
    }

    string[] GetCategoryNames () {
        string[] gameDataTypes = new string[selectedDatabase.myData.Count];
        gameDataList = selectedDatabase.myData;
        for (int i = 0; i < gameDataList.Count; i++) {
            gameDataTypes [i] = gameDataList[i].typeEnum.ToString ();
        }
        return gameDataTypes;
    }
}