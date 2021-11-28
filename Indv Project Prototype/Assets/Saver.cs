using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Saver : MonoBehaviour
{

    public StageHandler sh;

    public string path = "saved_sentences.txt";
    string root_path = "C:\\Users\\Rhodri\\Documents\\GitHub\\Diss\\Indv Project Prototype\\Assets\\";

    public void SaveToFile(Sentence[] s)
    {
        Debug.Log(root_path + path);
        using(StreamWriter sw = new StreamWriter(root_path + path,true))
        {
            foreach (Sentence s2 in s)
            {
                sw.WriteLine(s2.ToString());
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log("this");
            SaveToFile(sh.GetSentences());
        }
    }
}
