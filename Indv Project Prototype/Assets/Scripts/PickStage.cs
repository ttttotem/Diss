using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickStage : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject ws;

    public void SetSentence(string s)
    {
        text.text = s;
        this.enabled = true;
    }

    public void ChangeToWrite()
    {
        this.gameObject.SetActive(false);
        ws.SetActive(true);
    }
}
