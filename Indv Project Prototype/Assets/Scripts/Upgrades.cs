using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrades
{
    public string name;
    public int amount;
    public int maxValue;
    public int cost;
    [HideInInspector]
    public bool available = true;
}
