using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string _name;
    public int[] _enemyCounts;

    public Wave(string name, int[] enemyCounts)
    {
        _name = name;
        _enemyCounts = enemyCounts;
    }
}

//0 == noun
//1 == verb
//...
