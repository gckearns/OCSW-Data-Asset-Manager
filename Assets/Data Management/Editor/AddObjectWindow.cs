using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Text;

public class AddDataWindow : AddObjectWindow<GameItem>{}
public class AddAttributeWindow : AddObjectWindow<ItemAttribute>{}

public class AddObjectWindow<TObject> : EditorWindow where TObject : DataObject{

    public string objectName = "New Object";
    public string objectID = "NewObject";
    public static bool isOpen = false;
    public ObjectDatabase<TObject> database;
    public EditorWindow parentWindow;

    void OnDisable () {
        isOpen = false;
    }

    void OnGUI ()
    {
        isOpen = true;
        EditorGUILayout.BeginVertical (GUILayout.ExpandHeight(true));
        objectName = EditorGUILayout.TextField ("Name", objectName);
        EditorGUILayout.BeginHorizontal ();
        objectID = EditorGUILayout.TextField ("ID", objectID);
        if (GUILayout.Button ("Suggest")) {
            SuggestID ();
        }
        EditorGUILayout.EndHorizontal ();
        EditorGUILayout.EndVertical ();
        EditorGUILayout.BeginHorizontal ();
        if (GUILayout.Button ("OK")) {
            ValidateID ();
        }
        if (GUILayout.Button ("Cancel")) {
            Close ();
        }
        EditorGUILayout.EndHorizontal ();
        Event e = Event.current;
        //        Debug.Log ("Event: " + e.type.ToString());
        if (e.type == EventType.MouseUp) {
            EditorGUIUtility.keyboardControl = 0;
        }
    } 

    void ValidateID () {
        if (IsValidID(objectID)) {
            database.AddDataObject (objectName, objectID);
            parentWindow.Focus ();
            Close ();
            //            FocusWindowIfItsOpen<DataManagerWindow> ();
        } else {
            RemoveNotification ();
            ShowNotification (new GUIContent("ID already exists!"));
        }
    }

    void SuggestID() {
        StringBuilder sb = new StringBuilder ();
        List<char> charList = new List<char> (objectName.ToCharArray ());
        foreach (char c in charList) {
            if (!char.IsWhiteSpace(c)) {
                sb.Append (c);
            }
        }
        string tryID = sb.ToString ();
        int appendInt = 2;
        bool appended = false;
        while (!IsValidID(tryID)) {
            if (appended) {
                sb.Remove (sb.Length - 1, 1);
            }
            tryID = sb.Append(appendInt).ToString();
            appended = true;
            appendInt ++;
        }
        objectID = tryID;
    }

    bool IsValidID (string checkID){
        bool isValid = true;
        foreach (string objectID in database.myIDs) {
            if (objectID == checkID) {
                isValid = false;
            }
        } 
        return isValid;
    }
}
