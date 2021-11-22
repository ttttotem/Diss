using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    public string text;
    public int[] pos_correct;
}

[System.Serializable]
public class Sentences
{
    public Sentence[] sentences;
}
