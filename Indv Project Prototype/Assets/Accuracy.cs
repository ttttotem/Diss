using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accuracy : MonoBehaviour
{

    public TextAsset testFile;
    public string[] testSentences;

    private void Start()
    {
        testSentences = testFile.text.Split('\n');
    }

    public void CalculateAccuracy()
    {
        foreach (string temp_text in testSentences)
        {
            string loc = "";
            if (temp_text.Length > 0)
            {
                int i = temp_text.IndexOf(" ");

                //Split the locations from the setence
                loc = temp_text.Substring(0, i);
                string sentence = temp_text.Substring(i + 1);
                string[] temp = loc.Split(',');

                //Turn the locations into an array
                int[] locations = new int[temp.Length];
                for (int j = 0; j < temp.Length; j++)
                {
                    int val = 0;
                    if (int.TryParse(temp[j], out val))
                    {
                        locations[j] = val;
                    }
                    else
                    {
                        locations[j] = -2;
                    }
                }
            }
        }
    }
}
