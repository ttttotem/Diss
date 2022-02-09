using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public GameObject[] units;
    public float maxSpawnDelay;

    public TextMeshProUGUI text;

    public WaveUI waveUI;
    
    public List<Wave> waves = new List<Wave> ();

    public LevelTracker levelTracker;

    public int requiredWaves=10;
    int completedWaves=0;

    public Wave currentWave;

    public int GetCompletedWaves()
    {
        return completedWaves;
    }

    private void Start()
    {
        completedWaves = 0;
    }

    public void IncreaseCompletedWaves()
    {
        completedWaves += 1;
        currentWave = null;
        if(completedWaves >= requiredWaves)
        {
            //Tower Finished
            levelTracker.towerClear = true;
            waveUI.setWavesComplete(requiredWaves, requiredWaves);
        } else
        {
            waveUI.setWavesComplete(completedWaves,requiredWaves);
            StartWave();
        }
    }

    public void FailedWave()
    {
        if(currentWave == null)
        {
            return;
        }
        waves.Insert(0, currentWave);
        StartWave();
    }

    public void StartWave()
    {
        StopAllCoroutines();
        StartCoroutine(SpawnWave(5));
    }

    public void EndWave()
    {
        //Kill all units
        GameObject[] unitsAlive = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject unit in unitsAlive)
        {
            Destroy(unit);
        }
        if(currentWave != null)
        {
            waves.Insert(0, currentWave);
        }
    }

    public void RestartWave()
    {
        EndWave();
        StartWave();
    }

    public void AddWave(Wave wave)
    {
        if(wave != null && wave._enemyCounts.Length >0)
        {
            waves.Add(wave);
        }  
    }
    IEnumerator SpawnWave(int time)
    {
        if(waves.Count == 0 || waves == null)
        {
            waveUI.ResetUI();
            Debug.Log("no wave found");
            yield break;
        }
        Wave nextWave = waves[0];
        currentWave = nextWave;

        waves.RemoveAt(0);

        if (nextWave == null)
        {
            Debug.Log("Wave not found");
            yield break;
        }

        if (nextWave._enemyCounts == null)
        {
            Debug.Log("EnemyCounts not found");
            yield break;
        }

        int allies = nextWave._enemyCounts[0];
        int enemies1 = nextWave._enemyCounts[1];
        int enemies2 = nextWave._enemyCounts[2];
        
        waveUI.UpdateUI(nextWave);

        //Spawn Timer
        for (int i = time; i >= 0; i--)
        {
            waveUI.timerText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        waveUI.timerText.text = "";


        while (allies > 0 || enemies1 > 0 || enemies2 > 0)
        {
            int rand = Random.Range(0, 3);
            if (rand == 0 && allies > 0)
            {
                allies--;
                SpawnUnit(units[0]);
                waveUI.UnitSpawned(0);
                yield return new WaitForSeconds(Random.Range(0.8f, maxSpawnDelay));
            } else if(rand == 1 && enemies1 > 0)
            {
                enemies1--;
                SpawnUnit(units[1]);
                waveUI.UnitSpawned(1);
                yield return new WaitForSeconds(Random.Range(0.8f, maxSpawnDelay));
            } else if(rand == 2 && enemies2 >0)
            {
                enemies2--;
                SpawnUnit(units[2]);
                waveUI.UnitSpawned(2);
                yield return new WaitForSeconds(Random.Range(0.8f, maxSpawnDelay));
            }
        }
        waveUI.AllUnitsSpawned();
    }

    void SpawnUnit(GameObject unit)
    {
        Instantiate(unit,transform.position,transform.rotation);
    }

    public void AddWave(string paragraph,int medical=5)
    {
        var sentences = NLP.instance.SplitSentences(paragraph);
        int[] enemyCount = new int[7]; //0 = medical, 1 = noun, ...
        enemyCount[0] = medical;
        foreach (var sentence in sentences)
        {
            if(sentence.Length == 0 || sentence == "No more sentences")
            {
                continue;
            }
            NLP.instance.POSTagger(sentence);
            foreach(var posTag in NLP.instance.posTags)
            {
                if (posTag == "NN")
                {
                    enemyCount[1]++;
                } else if (posTag == "VB")
                {
                    enemyCount[2]++;
                } else
                {
                    enemyCount[3]++;
                }
            }
        }
        Wave newWave = new Wave(paragraph, enemyCount);
        AddWave(newWave);
    }
}
