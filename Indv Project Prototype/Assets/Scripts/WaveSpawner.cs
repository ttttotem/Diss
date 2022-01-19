using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{

    private string[] enemy;
    private string[] ally;

    public GameObject[] units;
    public float maxSpawnDelay;

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        enemy = new string[] { "hello", "this","is", "an","example", "whats" };
        ally = new string[] { "medicine", "vaxcine", "donor", "blood" };
        StartCoroutine(SpawnWave());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        int counter = 0;
        foreach (var enemy in enemy)
        {
            SpawnUnit(units[0]);
            yield return new WaitForSeconds(Random.Range(0.8f, maxSpawnDelay));
            if(Random.Range(0,2) == 0 && counter < ally.Length)
            {
                counter++;
                SpawnUnit(units[1]);
                yield return new WaitForSeconds(Random.Range(0.8f, maxSpawnDelay));
            }
        }
        //Some allies not spawned
         for (int i = counter; i < ally.Length; i++)
         {
            SpawnUnit(units[1]);
            yield return new WaitForSeconds(Random.Range(0.8f, maxSpawnDelay));
        }
    }

    void SpawnUnit(GameObject unit)
    {
        Instantiate(unit,transform.position,transform.rotation);
    }

    public void AddWave(string paragraph)
    {
        var sentences = NLP.instance.SplitSentences(paragraph);
        foreach (var sentence in sentences)
        {
            NLP.instance.POSTagger(sentence);
            int[] enemyCount = new int[7]; //0 = noun, 1 = verb, ...
            foreach(var posTag in NLP.instance.posTags)
            {
                if (posTag == "NN")
                {
                    enemyCount[0]++;
                } else if (posTag == "VB")
                {
                    enemyCount[1]++;
                } else
                {
                    enemyCount[2]++;
                }
            }
            Wave newWave = new Wave("Temp",enemyCount);
        }
    }

    public void ParseSubmittedSentence()
    {
        if(text.text == null || text.text == "Out of sentences")
        {
            return;
        } else
        {
            AddWave(text.text);
        }
    }
}
