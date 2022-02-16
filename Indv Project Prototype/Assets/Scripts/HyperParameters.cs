using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HyperParameters : MonoBehaviour
{

    bool complete = false;
    public string[] answer = { "7", "8", "9"};
    public InputField[] input;
    public Text text;
    public LevelTracker levelTracker;

    public AudioManager AudioManager;

    public void CheckAnswer()
    {
        if (!complete)
        {
            complete = true;
            for (int i = 0; i < input.Length; i++)
            {
                string guess = input[i].text;
                if (guess != answer[i])
                {
                    complete = false;
                }
            }
            if (complete)
            {
                Complete();
                levelTracker.ParamsTuned();
            } else
            {
                //Play wrong sound
                if (AudioManager != null)
                {
                    AudioManager.Play("error");
                }
            }
        }
    }

    public void Complete()
    {
        //Play correct sound
        if (AudioManager != null)
        {
            AudioManager.Play("correct");
        }

        for (int i = 0; i < input.Length; i++)
        {
            input[i].interactable = false;
            foreach(Text t in input[i].GetComponentsInChildren<Text>())
            {
                t.color = Color.green;
            }
        }
        string hyperParams = "";
        for (int i =0; i < answer.Length; i++)
        {
            hyperParams += answer[i] + ",";
        }
        hyperParams = hyperParams.Substring(0,hyperParams.Length - 1);
        text.text = hyperParams;
    }
}
