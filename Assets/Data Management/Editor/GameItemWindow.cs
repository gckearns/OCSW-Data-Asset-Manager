using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.UI;

public class GameItemWindow : EditorWindow {

    private int selectedToolType;
    private int selectedToolSort;
    private int selectedListItem;
    private Vector2 scrollPositionLeft;
    private Vector2 scrollPositionRight;

    public OcswDatabase myDatabase = null;
    private List<GameItemDatabase> databaseList = null;
    public GameItemDatabase selectedDatabase;
    private GameItem selectedGameData;
    private List<GameItem> gameDataList = new List<GameItem>();

    [MenuItem ("Data Managers/Game Data Manager")]
    static void Init() {
//        AttributeManagerWindow attWindow = (AttributeManagerWindow)EditorWindow.GetWindow (typeof(AttributeManagerWindow));
        GameItemWindow dataWindow = (GameItemWindow)EditorWindow.GetWindow (typeof(GameItemWindow));
//        attWindow.Close ();
//        dataWindow.Close ();
        dataWindow.Show ();
    }

    void OnEnable () {
        Debug.Log ("Data window enabled");
        Debug.Log (OcswManager.Database.ToString());
        if (myDatabase == null) {
                        Debug.Log ("myDatabase was null");
            myDatabase = OcswManager.Database;
        }
        if (databaseList == null) {
            //            Debug.Log ("Database List was null");
            databaseList = myDatabase.itemDatabases;
        }
        //        if (databaseList == null) {
////            Debug.Log ("Database List was null");
//            databaseList = GameItemManager.Databases;
//        }
    }

    void OnGUI (){
        if (AddDataWindow.isOpen) {
            LockedGUI ();
            FocusWindowIfItsOpen<AddDataWindow> ();
        } else {
            UnlockedGUI ();
        }
        Event e = Event.current;
//        Debug.Log ("Event: " + e.type.ToString());
        if (e.type == EventType.MouseUp) {
            EditorGUIUtility.keyboardControl = 0;
        }
    }

    void UnlockedGUI() {
        if (databaseList.Contains(null)) {
            databaseList = GameItemManager.Databases;
        }
        selectedToolType = GUILayout.Toolbar(selectedToolType, GetDataTypeNames());
        selectedDatabase = databaseList[selectedToolType];
        EditorGUILayout.BeginHorizontal (); { // Split list and data view
            EditorGUILayout.BeginVertical (); { // List of data
                selectedToolSort = GUILayout.Toolbar (selectedToolSort, new string[]{ "Name", "ID", "Category" });
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
                    if (!AddDataWindow.isOpen) {
                        AddDataWindow w = ScriptableObject.CreateInstance<AddDataWindow> ();
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
    }

    void LockedGUI() {
        if (databaseList.Contains(null)) {
            databaseList = GameItemManager.Databases;
        }
        GUILayout.Toolbar(selectedToolType, GetDataTypeNames());
        databaseList = GameItemManager.Databases;
        selectedDatabase = databaseList[selectedToolType];
        EditorGUILayout.BeginHorizontal (); { // Split list and data view
            EditorGUILayout.BeginVertical (); { // List of data
                GUILayout.Toolbar (selectedToolSort, new string[]{ "Name", "ID", "Category" });
                EditorGUILayout.BeginScrollView (scrollPositionLeft); {
                    EditorGUILayout.BeginHorizontal (); {
                        if (selectedDatabase.myData.Count > 0) {
                            if (selectedListItem >= selectedDatabase.myData.Count) {
                                selectedListItem = (selectedDatabase.myData.Count - 1);
                            }
                            if (selectedListItem < 0) {
                                selectedListItem = 0;
                            }
                            GUILayout.SelectionGrid (selectedListItem, GetDataNames (), 1);
                            GUILayout.SelectionGrid (selectedListItem, GetDataIDs (), 1);
                            GUILayout.SelectionGrid (selectedListItem, GetCategoryNames(), 1);
                            selectedGameData = gameDataList [selectedListItem];
                        }
                    } EditorGUILayout.EndHorizontal ();
                } EditorGUILayout.EndScrollView ();
                if (GUILayout.Button("Add")){
                }
            } EditorGUILayout.EndVertical (); 
            EditorGUILayout.BeginVertical (); { // data details
                if (selectedGameData != null) {
                    EditorGUILayout.BeginScrollView (scrollPositionRight);
                    {
                        selectedGameData.OnGUI ();   
                    }
                    EditorGUILayout.EndScrollView ();
                    if (GUILayout.Button ("Delete")) {
                    }
                }
            } EditorGUILayout.EndVertical ();
        } EditorGUILayout.EndHorizontal ();
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
            gameDataTypes [i] = gameDataList[i].category.ToString ();
        }
        return gameDataTypes;
    }
}