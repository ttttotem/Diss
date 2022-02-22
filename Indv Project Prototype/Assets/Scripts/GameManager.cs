using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    public bool SystemA = true;

    public bool FirstSystemComplete = false;
    public bool SecondSystemComplete = false;

    public double TimerLen = 30;
    public double Timer = 30;

    public string PlayerID = "Guest";

    [HideInInspector]
    public bool timing = false;

    public PopUp firstTimerPopUp;
    public PopUp secondTimerPopUp;

    public int score = 0;

    public float SysATime = 0;
    public float SysBTime = 0;

    [HideInInspector]
    public bool Switched = false;

    public void SwitchSystem()
    {
        SystemA = !SystemA;
        Switched = true;
    }
    
    public void StartTimer()
    {
        if(timing == false)
        {
            timing = true;
            Timer = TimerLen;
        }
    }

    private void FixedUpdate()
    {
        if(SystemA == true)
        {
            SysATime += Time.deltaTime;
        } else
        {
            SysBTime += Time.deltaTime;
        }
        if (timing == true)
        {
            if(Timer > 0)
            {
                Timer -= Time.deltaTime;

                if (Timer < 0)
                {
                    Timer = 0;
                }
            } else
            {
                if(FirstSystemComplete == false)
                {
                    FirstSystemComplete = true;
                    firstTimerPopUp.SetActive(true);
                    timing = false;
                } else if (SecondSystemComplete == false)
                {
                    SecondSystemComplete = true;
                    secondTimerPopUp.SetActive(true);
                    timing = false;
                }
            }
        }
    }

    public void CoinToss()
    {
        if(Random.Range(0,2) == 0)
        {
            SystemA = true;
        } else
        {
            SystemA = false;
        }
    }

    public void Start()
    {
        //Remove from live
        //Debug.Log("Removing player prefs, turn this off for live");
        //PlayerPrefs.DeleteAll();
        CoinToss();
    }

    public void Awake()
    {
        GameObject sceneCamObj = GameObject.Find("Main Camera");
        if (sceneCamObj != null)
        {
            // Should output the real dimensions of scene viewport
            Debug.Log(sceneCamObj.GetComponent<Camera>().pixelRect);
        }
        Screen.SetResolution(1280, 800, false);
        if (GameManager.GM == null)
        {
            GameManager.GM = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int scene = 0)
    {
        SceneManager.LoadScene(scene);
    }

    public void SetLevel(int level = 0, bool systemA = true)
    {
        string system = "SystemALevel";
        if (systemA == false)
        {
            system = "SystemBLevel";
        }
        if (PlayerPrefs.HasKey(system))
        {
           //Level can only go up
           if (PlayerPrefs.GetInt(system) < level)
           {
                PlayerPrefs.SetInt(system, level);
           }
        } else
        {
            //Stat is new
            PlayerPrefs.SetInt(system, level);
        }
    }
}
