using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    public bool SystemA = true;

    public string PlayerID = "Guest";

    [HideInInspector]
    public bool timing = false;

    public int score = 0;

    public float SysATime = 0;
    public float SysBTime = 0;

    public void SwitchSystem()
    {
        SystemA = !SystemA;
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
