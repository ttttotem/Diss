using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{

    public Text currentSystem;

    public void Start()
    {
        if (GameManager.GM.SystemA == true)
        {
            currentSystem.text = "Currently using system A";
        }
        else
        {
            currentSystem.text = "Currently using system B";
        }
    }

    public void OnEnable()
    {
        if (GameManager.GM.SystemA == true)
        {
            currentSystem.text = "Currently using system A";
        }
        else
        {
            currentSystem.text = "Currently using system B";
        }
    }

    public void SwitchSystem()
    {
        GameManager.GM.SwitchSystem();
    }

}
