using UnityEngine;
using System.Collections;
using UnityEditor;


public abstract class DatabaseEditor <TDatabase, TObject> : Editor 
    where TDatabase : ObjectDatabase <TObject>
    where TObject : DataObject {

    public override void OnInspectorGUI ()
    {
        if (targets.Length == 1) {
            base.OnInspectorGUI ();
        }
        if (GUILayout.Button ("RESET DATABASE")) {
            foreach (TDatabase database in targets) {
                database.ResetDatabase ();
            }
        }
    }
}

[CustomEditor (typeof (GameDatabase))]
[CanEditMultipleObjects]
public class GameDatabaseEditor : DatabaseEditor <GameDatabase, GameData> {
}

[CustomEditor (typeof (AttributeDatabase))]
[CanEditMultipleObjects]
public class AttributeDatabaseEditor : DatabaseEditor <AttributeDatabase, GameAttribute> {
}