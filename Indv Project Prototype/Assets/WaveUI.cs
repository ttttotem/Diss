using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{

    WaveSpawner waveSpawner;

    public Text waveName;
    public Text[] enemyCounts;

    public AllyEnd allyEnd;

    public void UpdateUI(Wave wave)
    {
        waveName.text = wave._name;
        allyEnd.SetRequired(wave._enemyCounts[0]);
        for (int i = 0; i< enemyCounts.Length; i++)
        {
            enemyCounts[i].text = wave._enemyCounts[i].ToString();
        }
    }

    public void ResetUI()
    {
        waveName.text = "Annotate sentences to add waves";
        for (int i = 0; i < enemyCounts.Length; i++)
        {
            enemyCounts[i].text = "0";
        }
    }
}
