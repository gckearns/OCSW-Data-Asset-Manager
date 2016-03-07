using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate bool DataChangeListener (System.Type dataObjectType, int index);

public class GameDataUtilities {
    
    public static event DataChangeListener DataRemovedListener;
    public static event DataChangeListener DataAddedListener;

    public static DataChangeListener dataRemovedListener { get { return DataRemovedListener; } }
    public static DataChangeListener dataAddedListener { get { return DataAddedListener; } }

    public static bool OnDBDataRemoved (System.Type t, int index) {
        return DataRemovedListener (t, index);
    }

    public static bool OnDBDataAdded (System.Type t, int index) {
        return DataAddedListener (t, index);
    }

    public static List<GameDataType> allDataTypes = new List<GameDataType> (
        new GameDataType[]{ GameDataType.Building, 
            GameDataType.Commodity}
        );
}
