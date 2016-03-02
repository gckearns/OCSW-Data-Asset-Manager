using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDataUtilities {

    public static event DataChangeListener DataRemovedListener;
    public static DataChangeListener dataRemovedListener { get { return DataRemovedListener; } }

    public static bool OnDBDataRemoved (System.Type t, int index) {
        return DataRemovedListener (t, index);
    }


    public static List<GameDataType> allDataTypes = new List<GameDataType> (
        new GameDataType[]{ GameDataType.Building, 
            GameDataType.Commodity}
        );
}
