using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TMPDetector : MonoBehaviour
{
    public TextMeshProUGUI text;

    public string LastClickedWord;
    public Color32 startColor = new Color32(0,0,0,255);
    public Color32 onClickColour = new Color32(20, 20, 20, 255);
    public Color32 hintColour = new Color32(200, 0, 160, 255);
    public Loader loader;
    List<int> selectedWords = new List<int>();
    public bool debug = false;
    int repeatFails = 0;

    public WaveSpawner waveSpawner;

    public SendData sendData;

    public Points points;

    //Problem as selected words not reseting when sentence changes
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var wordIndex = TMP_TextUtilities.FindIntersectingWord(text, Input.mousePosition, null);

            if (wordIndex != -1)
            {
                
                TMP_WordInfo wInfo = text.textInfo.wordInfo[wordIndex];
                LastClickedWord = wInfo.GetWord();
                if (selectedWords.Contains(wordIndex))
                {
                    selectedWords.Remove(wordIndex);
                    PaintWord(wInfo, startColor);
                } else
                {
                    selectedWords.Add(wordIndex);
                    if (debug)
                    {
                        Debug.Log(wordIndex);
                    }
                    PaintWord(wInfo, onClickColour);
                }
            }
        }
        if (Input.GetKey("h")){
            PaintCorrectWords();
        }
    }

    void PaintWord(TMP_WordInfo wInfo, Color32 color)
    {
        //change colour of clicked word
        Color32[] vertexColors = text.textInfo.meshInfo[0].colors32;
        for (int i = 0; i < wInfo.characterCount; i++)
        {
            int vertexIndex = text.textInfo.characterInfo[wInfo.firstCharacterIndex + i].vertexIndex;
            vertexColors[vertexIndex + 0] = color;
            vertexColors[vertexIndex + 1] = color;
            vertexColors[vertexIndex + 2] = color;
            vertexColors[vertexIndex + 3] = color;
        }
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }

    public int[] Get_Selected_Words()
    {
        return selectedWords.ToArray();
    }

    public void RepaintWords()
    {
        TMP_WordInfo[] wInfo = text.textInfo.wordInfo;
        foreach(TMP_WordInfo word in wInfo)
        {
            PaintWord(word, startColor);
        }
        selectedWords.Clear();
    }

    public void PaintCorrectWords()
    {
        RepaintWords();
        foreach(int i in points.correct_loc)
        {
            PaintWord(text.textInfo.wordInfo[i], hintColour);
        }
        selectedWords.Clear();
    }

    public void Reset_Words()
    {
        if(loader.Sendable == true)
        {
            sendData.addSentence(GenerateFormattedString());
        }
        
        if (points.knownSentence == true)
        {
            //Answer is known so make sure all words hit
            bool hits = points.check_Hits(selectedWords.ToArray());
            bool misses = points.check_Misses(selectedWords.ToArray());
            if (hits == true && misses == true)
            {
                //Answer correct
                Debug.Log("Correct");
            } else
            {
                Debug.Log("missed");
                if (loader == null)
                {
                    Debug.Log("No loader");
                }
                else
                { 
                    if (repeatFails <= 3)
                    {
                        loader.Load_Prev_Sentence();
                        RepaintWords();
                        if(repeatFails == 3)
                        {
                            PaintCorrectWords();
                        }
                        repeatFails++;
                        selectedWords.Clear();
                        return;
                    }
                }
            }
        }
        points.check_Loc(selectedWords.ToArray());
        string waveText = text.text;
        waveSpawner.AddWave(waveText, selectedWords.Count);
        selectedWords.Clear();
        if(loader == null)
        {
            Debug.Log("No loader");
        } else
        {
            repeatFails = 0;
            loader.Load_Next_Sentence();
        }
    }

    //Might need to add merging words next to each other login if dont go iloc
    public string GenerateFormattedString()
    {
        string mainSentence = text.text.ToString();
        string superSentence = "";
        int currentLoc = 0;
        TMP_WordInfo[] selectedWordsInfo = new TMP_WordInfo[selectedWords.Count];
        for(int i = 0; i<selectedWords.Count; i++)
        {
            selectedWordsInfo[i] = text.textInfo.wordInfo[selectedWords[i]];
        }
        foreach (TMP_WordInfo word in selectedWordsInfo)
        {
            if(word.firstCharacterIndex < mainSentence.Length && word.firstCharacterIndex - currentLoc >= 0)
            {
                superSentence += mainSentence.Substring(currentLoc, word.firstCharacterIndex - currentLoc);
                superSentence += "<start: medical>" + word.GetWord() + "<end>";
                currentLoc = word.lastCharacterIndex + 1;
            }
        }
        if(currentLoc < mainSentence.Length)
        {
            superSentence += mainSentence.Substring(currentLoc, mainSentence.Length - currentLoc);
        }
        return superSentence;
    }
}
