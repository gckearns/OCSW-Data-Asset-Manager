using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class GameDatabase : ObjectDatabase <GameData>  {

    public override GameData GetDefaultObject ()
    {
        GameData gd = ScriptableObject.CreateInstance <GameData> ();
        gd.category = GameDataCategory.Services;
        return new GameData ();
    }
}
