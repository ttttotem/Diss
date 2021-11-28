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
    public List<Sentence> sentences;
    public TMPDetector tmpd;
    int counter;

    private void Start()
    {
        counter = 0;
    }

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

    public void SubmitSentence()
    {
        counter += 1;
        Sentence s = new Sentence(output_text.text, tmpd.Get_Selected_Words());
        sentences.Add(s);
        if (counter > 4)
        {
            EvaluateSentences();
        } else
        {
            output_text.text = "Enter a new sentence...";
            SetWriteStageActive();
        }
    }

    public void EvaluateSentences()
    {
        int result = GetComponent<Eval>().Evaluate(sentences.ToArray());
    }

    public Sentence[] GetSentences()
    {
        return sentences.ToArray();
    }

}
