using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerB : MonoBehaviour
{
    public int level = 0;
    public Button[] levels;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("SystemBLevel"))
        {
            level = PlayerPrefs.GetInt("SystemBLevel");
        }
    }

    public void UpdateLevels()
    {
        if (PlayerPrefs.HasKey("SystemBLevel"))
        {
            level = PlayerPrefs.GetInt("SystemBLevel");
        }
        int counter = 0;
        foreach (Button buttonLevel in levels)
        {
            if (counter <= level)
            {
                buttonLevel.interactable = true;
            }
            else
            {
                buttonLevel.interactable = false;
            }
            counter += 1;
        }
    }

    private void OnEnable()
    {
        UpdateLevels();
    }
}
