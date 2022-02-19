using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEnd : MonoBehaviour
{

    public static EnemyEnd instance;
    public WaveUI waveUI;

    AudioManager am;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one build manager in scene");
            return;
        }
        instance = this;
    }


    public int lives = 10;
    private int currentLives = 10;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        currentLives = lives;
        text.text = currentLives.ToString();
        am = FindObjectOfType<AudioManager>();
    }

    public void LoseLives(int lives)
    {
        currentLives -= lives;
        if (currentLives < 0)
        {
            currentLives = 0;
        }

        if(am != null)
        {
            am.Play("shortBuzz");
        }

        text.text = currentLives.ToString();
    }

    public void ResetLives()
    {
        currentLives = lives;
        text.text = lives.ToString();
    }

    public bool CheckPassed()
    {
        if(currentLives > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
