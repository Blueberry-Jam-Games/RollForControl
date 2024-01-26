using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "RFC/LootBox", order = 1)]
public class LootBoxRoll : ScriptableObject
{
    public List<LootBoxItem> rolls;
    public String LootBoxName;
}

[System.Serializable]
public class LootBoxItem
{
    public string name;
    public Sprite image;
    // In the range 0 - 1
    public float percentage;
}
