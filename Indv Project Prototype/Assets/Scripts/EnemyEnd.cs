using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEnd : MonoBehaviour
{

    public static EnemyEnd instance;
    public WaveUI waveUI;

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
        text.text = currentLives.ToString();
    }

    public void LoseLives(int lives)
    {
        currentLives -= lives;
        if (currentLives < 0)
        {
            currentLives = 0;
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
