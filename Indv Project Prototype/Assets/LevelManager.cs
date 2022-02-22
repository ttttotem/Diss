using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public int level = 0;
    public Button[] levels;
    public InputField username;


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("SystemALevel"))
        {
            level = PlayerPrefs.GetInt("SystemALevel");
        }
        else
        {
            level = 0;
        }
    }

    public void UpdateLevels()
    {
        if (PlayerPrefs.HasKey("SystemALevel"))
        {
            level = PlayerPrefs.GetInt("SystemALevel");
        } else
        {
            level = 0;
        }
        int counter = 0;
        foreach(Button buttonLevel in levels)
        {
            if(counter <= level)
            {
                buttonLevel.interactable = true;
            } else
            {
                buttonLevel.interactable = false;
            }
            counter += 1;
        }
        username.text = GameManager.GM.PlayerID;
    }

    private void OnEnable()
    {
        UpdateLevels();
    }
}
