using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    public string text;
    public int[] pos_correct;

    public Sentence(string s, int[] pos)
    {
        this.text = s;
        this.pos_correct = pos;
    }

    override
    public string ToString()
    {
        string positions = ", [";
        foreach (int i in pos_correct)
        {
            positions += i + "+";
        }
        positions += "]";
        return(text + " " + positions);
    }
}

[System.Serializable]
public class Sentences
{
    public Sentence[] sentences;
}
