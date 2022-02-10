using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public Button systemA;
    public Button systemB;
    public Text text;
    public Text currentSystem;

    bool Switched = false;

    public void Start()
    {
        systemA.interactable = false;
        systemB.interactable = false;

        if (GameManager.GM.SystemA == true)
        {
            currentSystem.text = "Currently using system A";
        }
        else
        {
            currentSystem.text = "Currently using system B";
        }
        OnEnable();
    }

    private void OnEnable()
    {
        if(GameManager.GM.FirstSystemComplete == true && GameManager.GM.SecondSystemComplete == true)
        {
            //Finished using both systems
            text.text = "Thank you for taking part in this investigation. Feel free to use either system";
            systemA.interactable = true;
            systemB.interactable = true;
            return;
        }

        if(Switched == true)
        {
            return;
        }


        if(GameManager.GM.FirstSystemComplete == true && GameManager.GM.SystemA == true)
        {
            //Finished using System A and onto System B
            text.text = "You have finished using system A. Please press the button to try using system B next";
            systemA.interactable = false;
            systemB.interactable = true;
        } else if (GameManager.GM.FirstSystemComplete == true && GameManager.GM.SystemA == false)
        {
            //System B finished onto system A
            text.text = "You have finished using system B. Please press the button to try using system A next";
            systemA.interactable = true;
            systemB.interactable = false;
        } else
        {
            //Not finished
            text.text = "You cannot change system until the 10 minute grace period has passed";
            systemA.interactable = false;
            systemB.interactable = false;
        }
    }

    public void SetSystemA(bool val)
    {
        GameManager.GM.SystemA = val;
        Switched = true;
        if(val == true)
        {
            currentSystem.text = "Currently using system A";
        } else
        {
            currentSystem.text = "Currently using system B";
        }
    }

}
