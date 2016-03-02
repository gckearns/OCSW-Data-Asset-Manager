using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.UI;

public class DataManagerWindow : EditorWindow {

    private int selectedToolType;
    private int selectedToolSort;
    private int selectedListItem;
    private Vector2 scrollPositionLeft;
    private Vector2 scrollPositionRight;

//    public GameDataManager manager = null;
    private List<GameDatabase> databaseList = null;
    public GameDatabase selectedDatabase;
    private GameData selectedGameData;
    private List<GameData> gameDataList = new List<GameData>();

    [MenuItem ("Data Managers/Game Data Manager")]
    static void Init() {
        AttributeManagerWindow attWindow = (AttributeManagerWindow)EditorWindow.GetWindow (typeof(AttributeManagerWindow));
        DataManagerWindow dataWindow = (DataManagerWindow)EditorWindow.GetWindow (typeof(DataManagerWindow));
        attWindow.Close ();
        dataWindow.Show ();
    }

    void OnEnable () {
        Debug.Log ("Data window enabled");
//        if (manager == null) {
//            Debug.Log ("Manager was null");
//            manager = new GameDataManager(); // do i really need this?
//        }
        if (databaseList == null) {
            Debug.Log ("Database List was null");
            databaseList = GameDataManager.Databases;
        }
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
        selectedToolType = GUILayout.Toolbar(selectedToolType, GetDataTypeNames());
//        selectedDatabase = gameDictionary[GameDataUtilities.allDataTypes [selectedToolType]];
        selectedDatabase = databaseList[selectedToolType];
//        while (selectedDatabase == null) {
//            databaseList = GameDataManager.Databases;
//            selectedDatabase = databaseList[selectedToolType];
//        }
        EditorGUILayout.BeginHorizontal (); { // Split list and data view
            EditorGUILayout.BeginVertical (); { // List of data
                selectedToolSort = GUILayout.Toolbar (selectedToolSort, new string[]{ "Name", "ID", "Category" });
                scrollPositionLeft = EditorGUILayout.BeginScrollView (scrollPositionLeft); {
                    EditorGUILayout.BeginHorizontal (); {
//                        if (selectedDatabase.dictionary.Count > 0) {
                        if (selectedDatabase.myData.Count > 0) {
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
                scrollPositionRight = EditorGUILayout.BeginScrollView (scrollPositionRight); {
                    if (selectedGameData != null) {
                        selectedGameData.OnGUI ();
                    }
                } EditorGUILayout.EndScrollView ();
                if (GUILayout.Button("Delete")){
                    if (selectedGameData != null) {
                        // Put a warning dialogue here
                        selectedDatabase.RemoveData (selectedGameData.dataObjectID);
                        selectedGameData = null;
                        selectedListItem --;
                    }
                    if (selectedListItem < 0) {
                        selectedListItem = 0;
                    }
                }
            } EditorGUILayout.EndVertical ();
        } EditorGUILayout.EndHorizontal ();
    }

    void LockedGUI() {
        GUILayout.Toolbar(selectedToolType, GetDataTypeNames());
        databaseList = GameDataManager.Databases;
//        selectedDatabase = gameDictionary[GameDataUtilities.allDataTypes [selectedToolType]];
        selectedDatabase = databaseList[selectedToolType];
        EditorGUILayout.BeginHorizontal (); { // Split list and data view
            EditorGUILayout.BeginVertical (); { // List of data
                GUILayout.Toolbar (selectedToolSort, new string[]{ "Name", "ID", "Category" });
                EditorGUILayout.BeginScrollView (scrollPositionLeft); {
                    EditorGUILayout.BeginHorizontal (); {
                        if (selectedDatabase.myData.Count > 0) {
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
                EditorGUILayout.BeginScrollView (scrollPositionRight); {
                    if (selectedGameData != null) {
                        selectedGameData.OnGUI ();
                    }
                } EditorGUILayout.EndScrollView ();
                if (GUILayout.Button("Delete")){
                }
            } EditorGUILayout.EndVertical ();
        } EditorGUILayout.EndHorizontal ();
//        FocusWindowIfItsOpen<AddDataWindow> ();
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