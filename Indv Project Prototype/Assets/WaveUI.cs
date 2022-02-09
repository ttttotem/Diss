using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{

    public WaveSpawner waveSpawner;

    public Text waveName;
    public Text[] enemyCounts;

    public Text wavesComplete;

    public AllyEnd allyEnd;
    public EnemyEnd EnemyEnd;

    Wave currentWave;
    GameObject[] unitsAlive;
    public float checkAliveTimer = 0.5f;
    float currentTime;

    int[] currentWaveEnemies = new int[7];

    public Text timerText;

    bool UnitsSpawning = false;
    bool WaveStarted = false;

    public void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime < 0)
        {
            if (CheckAllUnitsDead() == true && currentWave != null && UnitsSpawning == false && WaveStarted == true)
            {
                if(allyEnd.CheckPassed() && EnemyEnd.CheckPassed())
                {
                    waveSpawner.IncreaseCompletedWaves();

                    UnitsSpawning = true;
                    WaveStarted = false;
                } else
                {
                    waveSpawner.FailedWave();

                    UnitsSpawning = true;
                    WaveStarted = false;
                }
            }
            currentTime = checkAliveTimer;
        }
    }

    public void setWavesComplete(int complete, int required)
    {
        wavesComplete.text = complete + "/" + required;
    }

    public bool CheckAllUnitsDead()
    {
        unitsAlive = GameObject.FindGameObjectsWithTag("Enemy");
        if (unitsAlive.Length == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UnitSpawned(int unitType)
    {
        WaveStarted = true;
        if (currentWaveEnemies[unitType] < 0)
        {
            return;
        }
        currentWaveEnemies[unitType] -= 1;
        enemyCounts[unitType].text = currentWaveEnemies[unitType].ToString();
    }

    public void AllUnitsSpawned()
    {
        UnitsSpawning = false;
    }

    public void UpdateUI(Wave wave)
    {
        currentWave = wave;
        waveName.text = wave._name;
        allyEnd.SetRequired(wave._enemyCounts[0]);
        EnemyEnd.ResetLives();
        for (int i = 0; i< enemyCounts.Length; i++)
        {
            enemyCounts[i].text = wave._enemyCounts[i].ToString();
            currentWaveEnemies[i] = wave._enemyCounts[i];
        }
    }

    public void ResetUI()
    {
        waveName.text = "Annotate sentences to add waves";
        for (int i = 0; i < enemyCounts.Length; i++)
        {
            enemyCounts[i].text = "0";
        }
        wavesComplete.text = waveSpawner.GetCompletedWaves() + "/" + waveSpawner.requiredWaves;
        timerText.text = "";
    }

    public void SetSpeed(float speed)
    {
        Time.timeScale = speed;
    }
}
