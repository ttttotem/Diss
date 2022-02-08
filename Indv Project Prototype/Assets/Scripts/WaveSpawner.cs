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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine(SpawnWave());
        }
    }

    public void AddWave(Wave wave)
    {
        if(wave != null && wave._enemyCounts.Length >0)
        {
            waves.Add(wave);
        }  
    }

    IEnumerator SpawnWave()
    {
        if(waves.Count == 0 || waves == null)
        {
            waveUI.ResetUI();
            Debug.Log("no wave found");
            yield break;
        }
        Wave nextWave = waves[0];
        waves.RemoveAt(0);

        if(nextWave == null || nextWave._enemyCounts.Length == 0)
        {
            Debug.Log("Wave has no enemies");
            yield break;
        }

        int allies = nextWave._enemyCounts[0];
        int enemies1 = nextWave._enemyCounts[1];
        int enemies2 = nextWave._enemyCounts[2];
        
        waveUI.UpdateUI(nextWave);

        while (allies > 0 || enemies1 > 0 || enemies2 > 0)
        {
            int rand = Random.Range(0, 3);
            if (rand == 0 && allies > 0)
            {
                allies--;
                SpawnUnit(units[0]);
                yield return new WaitForSeconds(Random.Range(0.8f, maxSpawnDelay));
            } else if(rand == 1 && enemies1 > 0)
            {
                enemies1--;
                SpawnUnit(units[1]);
                yield return new WaitForSeconds(Random.Range(0.8f, maxSpawnDelay));
            } else if(rand == 2 && enemies2 >0)
            {
                enemies2--;
                SpawnUnit(units[2]);
                yield return new WaitForSeconds(Random.Range(0.8f, maxSpawnDelay));
            }
            
        }
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
            Debug.Log(sentence);
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
