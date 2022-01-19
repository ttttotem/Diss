using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WriteStage : MonoBehaviour
{

    public GameObject ps;
    public TMP_InputField text;

    public void ChangeToPick()
    {
        string s = text.text;
        ps.SetActive(true);
        ps.GetComponent<PickStage>().SetSentence(s);
        this.gameObject.SetActive(false);
    }
}
