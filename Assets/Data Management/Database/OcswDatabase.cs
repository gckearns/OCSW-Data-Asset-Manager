using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class OcswDatabase : ScriptableObject {

    public delegate bool DataChangeListener (System.Type dataObjectType, int index);

    [SerializeField]
    public event DataChangeListener DataRemovedListener;
    [SerializeField]
    public event DataChangeListener DataAddedListener;

    public DataChangeListener dataRemovedListener { get { return DataRemovedListener; } }
    public DataChangeListener dataAddedListener { get { return DataAddedListener; } }

    [SerializeField]
    public List<GameItemDatabase> itemDatabases = null;
    [SerializeField]
    public List<AttributeDatabase> attDatabases = null;

    public bool OnDBDataRemoved (System.Type t, int index) {
        return DataRemovedListener (t, index);
    }

    public bool OnDBDataAdded (System.Type t, int index) {
        return DataAddedListener (t, index);
    }

    public void OnEnable () {
        itemDatabases = GameItemManager.Databases;
        attDatabases = GameAttributeManager.Databases;
    }

    public void ResetDatabase () {
        foreach (var item in itemDatabases) {
            item.ResetDatabase ();
        }
        foreach (var item in attDatabases) {
            item.ResetDatabase ();
        }
        DataAddedListener = null; // dont think this is necessary cuz of the above foreach
        DataRemovedListener = null; // dont think this is necessary cuz of the above foreach
    }
}
