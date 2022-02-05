using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HyperParameters : MonoBehaviour
{

    bool complete = false;
    public string[] answer = { "7", "8", "9"};
    public Dropdown[] dd;
    public Button btn;
    public Text text;
    public LevelTracker levelTracker;

    public void CheckAnswer()
    {
        if (!complete)
        {
            complete = true;
            for (int i = 0; i < dd.Length; i++)
            {
                string guess = dd[i].options[dd[i].value].text;
                if (guess != answer[i])
                {
                    complete = false;
                }
            }
            if (complete)
            {
                Complete();
                levelTracker.ParamsTuned();
            }
        }
    }

    public void Complete()
    {
        for (int i = 0; i < dd.Length; i++)
        {
            dd[i].interactable = false;
            dd[i].GetComponentInChildren<Text>().color = Color.green;
        }
        btn.interactable = false;
        string hyperParams = "";
        for (int i =0; i < answer.Length; i++)
        {
            hyperParams += answer[i] + ",";
        }
        hyperParams = hyperParams.Substring(0,hyperParams.Length - 1);
        text.text = hyperParams;
    }
}
