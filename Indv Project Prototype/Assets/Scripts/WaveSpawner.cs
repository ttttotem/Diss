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

    AudioManager am;
    Money money;

    public int GetCompletedWaves()
    {
        return completedWaves;
    }

    private void Start()
    {
        completedWaves = 0;
        am = FindObjectOfType<AudioManager>();
        money = FindObjectOfType<Money>();
    }

    public void IncreaseCompletedWaves()
    {

        completedWaves += 1;
        currentWave = null;
        if(completedWaves >= requiredWaves)
        {
            //Tower Finished
            levelTracker.towerClear = true;
            if(am != null)
            {
                am.Play("correct");
            }
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
        if(currentWave._enemyCounts == null)
        {
            return;
        }
        money.AddMoney(100);
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
            if(currentWave._enemyCounts != null)
            {
                if(currentWave._enemyCounts.Length >= 5)
                {
                    waves.Insert(0, currentWave);
                }
            }
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
            nextWave = null;
            yield break;
        }

        if (nextWave._enemyCounts.Length < 5)
        {
            Debug.Log("EnemyCounts not formed properly");
            nextWave = null;
            yield break;
        }

        int allies = nextWave._enemyCounts[0];
        int enemies1 = nextWave._enemyCounts[1];
        int enemies2 = nextWave._enemyCounts[2];
        int enemies3 = nextWave._enemyCounts[3];
        int enemies4 = nextWave._enemyCounts[4];

        waveUI.UpdateUI(nextWave);

        //Spawn Timer
        for (int i = time; i >= 1; i--)
        {
            waveUI.timerText.text = i.ToString();

            //Play timer sound
            if(am != null)
            {
                am.Play("timerTick");
            }

            yield return new WaitForSeconds(1f);
        }
        waveUI.timerText.text = "";

        while (allies > 0 || enemies1 > 0 || enemies2 > 0 || enemies3 > 0 || enemies4 > 0)
        {
            int rand = Random.Range(0, 5);
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
            else if (rand == 3 && enemies3 > 0)
            {
                enemies3--;
                SpawnUnit(units[3]);
                waveUI.UnitSpawned(3);
                yield return new WaitForSeconds(Random.Range(0.8f, maxSpawnDelay));
            }
            else if (rand == 4 && enemies4 > 0)
            {
                enemies4--;
                SpawnUnit(units[4]);
                waveUI.UnitSpawned(4);
                yield return new WaitForSeconds(Random.Range(0.8f, maxSpawnDelay));
            }
        }
        waveUI.AllUnitsSpawned();
    }

    void SpawnUnit(GameObject unit)
    {
        //Play spawn sound
        if(am != null)
        {
            am.Play("shortBuzz");
        }
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
            foreach(var posTag in NLP.instance.posTags)
            {
                if (posTag == "NN")
                {
                    enemyCount[1]++;
                } else if (posTag == "VB")
                {
                    enemyCount[2]++;
                } else if (posTag == "JJ")
                {
                    enemyCount[3]++;
                } else if (posTag == "DT")
                {
                    enemyCount[4]++;
                }
            }
        }

        //Scale number of enemies down
        for (int i = 0; i < 5; i++)
        {
            enemyCount[i] = (enemyCount[i] / 10) + 1;
        }
        
        Wave newWave = new Wave(paragraph, enemyCount);
        AddWave(newWave);
    }
}
