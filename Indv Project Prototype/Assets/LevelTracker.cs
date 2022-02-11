using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTracker : MonoBehaviour
{
    int submissions;
    public int RequiredSubmissions = 10;
    bool paramsTuned = false;
    public bool towerClear = false;
    int accuracy = 0;
    public Text text;

    int tower = 0;
    int subs = 0;
    int paramsTune = 0;

    public Text[] falses;


    public void Start()
    {
        submissions = 0;
        paramsTuned = false;
        towerClear = false;
        accuracy = 0;

        tower = 0;
        subs = 0;
        paramsTune = 0;
    }

    public void IncreaseSubmission()
    {
        submissions++;
    }

    public void TowerCleared()
    {
        towerClear = true;
    }

    public void ParamsTuned()
    {
        paramsTuned = true;
    }
    
    public void Recalculate()
    {
        accuracy = 0;
        if (paramsTuned == true && paramsTune == 0)
        {
            //Hyper params tuned
            SetFalseTextTrue(1);
            paramsTune = Random.Range(10, 33);
        }

        if (towerClear == true && tower == 0)
        {
            //Tower passed
            SetFalseTextTrue(2);
            tower = Random.Range(10, 33);
        }

        if(submissions <= RequiredSubmissions)
        {
            if(submissions > 0)
            {
                subs = (int)(((float)submissions / (float)RequiredSubmissions) * 33);
            }
        }
        else
        {
            if(subs == 0)
            {
                subs = Random.Range(10, 33);
            }
            else
            {
                //Required submissions met
                SetFalseTextTrue(0);
                subs = 30;
            }
        }

        accuracy = accuracy + subs + tower + paramsTune;

        if (towerClear == true && paramsTuned == true && submissions >= RequiredSubmissions)
        {
            accuracy = 100;
        }

        text.text = "Test Accuracy: " + accuracy + "%";
    }

    void SetFalseTextTrue(int i)
    {
        if (i < falses.Length)
        {
            if (falses[i] != null)
            {
                falses[i].text = "True";
                falses[i].color = Color.green;
            }
        }
    }

    private void OnEnable()
    {
        Recalculate();
    }
}
