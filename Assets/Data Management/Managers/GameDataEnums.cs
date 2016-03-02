using UnityEngine;
using System.Collections;
using System;
[Serializable]
public enum GameDataType
{
    None = 0,
    Building,
    Commodity
}
[Serializable]
public enum GameDataCategory
{
    None = 0,
    Housing,
    Industry,
    Services
}
[Serializable]
public enum GameAttributeType
{
    Int = 0,
    Float,
    IntSlider
}