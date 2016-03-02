using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.UI;

public abstract class GenericManagerWindow <TWindow, TManager, TDatabase, TObject> : EditorWindow 
    where TWindow : AddObjectWindow <TObject>
    where TManager : DatabaseManager <TDatabase, TObject>
    where TDatabase : ObjectDatabase <TObject> 
    where TObject : DataObject {

    private int selectedToolType;
    private int selectedToolSort;
    private int selectedListItem;
    private Vector2 scrollPositionLeft;
    private Vector2 scrollPositionRight;

//    public TManager manager = new TManager();
//    public GameAttributeManager manager = new GameAttributeManager();
    //    private GameDataDictionary gameDictionary;
    private List<TDatabase> databases = null;
    public TDatabase selectedDatabase;
    private TObject selectedGameData;
    private List<TObject> gameDataList = new List<TObject>();

    [MenuItem ("Data Managers/Generic Manager")]
    static void Init() {
        GenericManagerWindow <TWindow, TManager, TDatabase, TObject> gmw = (GenericManagerWindow <TWindow, TManager, TDatabase, TObject>) 
            EditorWindow.GetWindow (typeof(GenericManagerWindow <TWindow, TManager, TDatabase, TObject>));
//        DataManagerWindow dataWindow = (DataManagerWindow)EditorWindow.GetWindow (typeof(DataManagerWindow));
//        dataWindow.Close ();
        gmw.Show ();
    }

    void OnEnable () {
//        if (databases == null) {
//            databases = TManager.Databases;
//        }
    }

    void OnGUI (){
        selectedToolType = GUILayout.Toolbar(selectedToolType, GetDataTypeNames());
        selectedDatabase = databases[selectedToolType];
        EditorGUILayout.BeginHorizontal (); { // Split list and data view
            EditorGUILayout.BeginVertical (); { // List of data
                selectedToolSort = GUILayout.Toolbar (selectedToolSort, new string[]{ "Name", "ID", "Type" });
                scrollPositionLeft = EditorGUILayout.BeginScrollView (scrollPositionLeft); {
                    EditorGUILayout.BeginHorizontal (); {
                        if (selectedDatabase.myData.Count > 0) {
                            selectedListItem = GUILayout.SelectionGrid (selectedListItem, GetDataNames (), 1);
                            selectedListItem = GUILayout.SelectionGrid (selectedListItem, GetDataIDs (), 1);
                            selectedListItem = GUILayout.SelectionGrid (selectedListItem, GetCategoryNames(), 1);
                            selectedGameData = gameDataList [selectedListItem];
                        }
                    } EditorGUILayout.EndHorizontal ();
                } EditorGUILayout.EndScrollView ();
                if (GUILayout.Button("Add")){
//                    if (!TWindow.isOpen) {
//                        //                        selectedDatabase.AddGameData (selectedDatabase.myData.Capacity.ToString());
//                        TWindow w = ScriptableObject.CreateInstance<TWindow> ();
//                        w.database = selectedDatabase;
//                        w.ShowUtility ();
//                    }
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
//            gameDataTypes [i] = gameDataList[i].typeEnum.ToString ();
        }
        return gameDataTypes;
    }
}