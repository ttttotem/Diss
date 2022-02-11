using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageHandler : MonoBehaviour
{
    public TMP_InputField input_text;
    public TextMeshProUGUI output_text;
    public GameObject ws;
    public GameObject ps;
    public TMPDetector tmpd;

    public void SetPickStageActive()
    {
        output_text.text = input_text.text;
        ps.SetActive(true);
        ws.SetActive(false);
    }

    public void SetWriteStageActive()
    {
        input_text.text = output_text.text;
        tmpd.Reset_Words();
        ws.SetActive(true);
        ps.SetActive(false);
    }
}
