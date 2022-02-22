using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpEntropy;
using SharpEntropy.IO;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Voxell.NLP.Parser;
using Voxell.NLP.Util;
using UnityEngine;
using Voxell;
using Voxell.NLP.NameFind;
using Voxell.Inspector;
using TMPro;
using System.IO;

public class NLPOrgs : MonoBehaviour
{
    [StreamingAssetFolderPath] public string nameFinderModel;
    public string[] models = new string[]
    {"organization"};

    public TextAsset knownSentences;

    public TextMeshProUGUI text_helper;


    List<int> loc = new List<int>();

    private EnglishNameFinder nameFinder;

    public void Start()
    {
        nameFinder = new EnglishNameFinder(FileUtilx.GetStreamingAssetFilePath(nameFinderModel));
        //GenerateKnownSentences();
        //Testing();
    }

    public string ParseSentence(string s)
    {
        string string_with_tags = nameFinder.GetNames(models, s);
        return ParseTagLoc(string_with_tags);
    }

    public string ParseTagLoc(string s)
    {
        loc.Clear();
        text_helper.text = s;
        text_helper.ForceMeshUpdate(true);
        string superstring = "";
        int i = 0;
        bool chaining = false;
        var wordInfoSnapshot = text_helper.textInfo.wordInfo;
        foreach (TMP_WordInfo wInfo in wordInfoSnapshot)
        {
            if(i > text_helper.textInfo.wordCount)
            {
                Debug.Log("this");
                continue;
            }
            if(wInfo.characterCount > 0)
            {
                string t = wInfo.GetWord();
                //if (!s.Contains(t))
                //{
                    //Debug.Log("Skipped");
                    //continue;
                //}
                if (t.Contains("STARTCODE001"))
                {
                    if (t.Contains("ENDCODE002"))
                    {
                        loc.Add(i);
                        chaining = false;
                    } else
                    {
                        loc.Add(i);
                        chaining = true;
                    }
                }
                else if (t.Contains("ENDCODE002"))
                {
                    loc.Add(i);
                    chaining = false;
                }
                else
                {
                    if (chaining == true)
                    {
                        loc.Add(i);
                    }
                    
                    superstring += t;
                }
                i++;
            }
        }
        return superstring;
    }

    public void ClearLoc()
    {
        loc.Clear();
    }

    public int[] GetLoc()
    {
        if(loc == null)
        {
            int [] empty = new int[] {0};
            return empty;
        }
        return loc.ToArray();
    }

    public void GenerateKnownSentences()
    {
        string[] sentences = knownSentences.text.Split('\n');
        List<string> newSent = new List<string>();
        string temp;
        string temp_sentence;

        foreach (string sentence in sentences)
        {
            temp = "";
            temp_sentence = sentence;
            loc.Clear();
            ParseTagLoc(temp_sentence);

            int counter = 0;

            foreach(int j in loc)
            {
                counter++;
                if(counter == loc.Count)
                {
                    temp += j;
                }
                else
                {
                    temp += j + ",";
                }
            }
            loc.Clear();
            //Remove tags

            var temp_sentence1 = temp_sentence.Replace("STARTCODE001", "");
            var temp_sentence2 = temp_sentence1.Replace("ENDCODE002", "");

            if (temp.Length > 0 && temp_sentence2.Length > 0)
            {
                Debug.Log(temp);
                temp_sentence2 = temp + " " + temp_sentence2;
                newSent.Add(temp_sentence2);
            }
        }



        //Save file
        string path = "Known_Sentences.txt";
        string root_path = "C:\\Users\\Rhodri\\Documents\\GitHub\\Diss\\Indv Project Prototype\\Assets\\";

        using (StreamWriter sw = new StreamWriter(root_path + path, true))
        {
            foreach (string s2 in newSent)
            {
                sw.Write(s2);
            }
        }
    }

    public void ParseString(string s)
    {
        string[] split = s.Split(new char[] {' ','/',')'});
        int i = 0;
        bool chaining = false;
        loc.Clear();
        foreach (string word in split)
        {
            if (word.Length == 1)
            {
                if (char.IsPunctuation(word[0]) || char.IsWhiteSpace(word[0]))
                {
                    continue;
                }
            }

            if (word.Length > 0)
            {
                string t = word;
                if (t.Contains("STARTCODE001"))
                {
                    if (t.Contains("ENDCODE002"))
                    {
                        loc.Add(i);
                        chaining = false;
                    }
                    else
                    {
                        loc.Add(i);
                        chaining = true;
                    }
                }
                else if (t.Contains("ENDCODE002"))
                {
                    loc.Add(i);
                    chaining = false;
                }
                else
                {
                    if (chaining == true)
                    {
                        loc.Add(i);
                    }
                }
                i++;
            }
        }
    }

    public void Testing()
    {
        Debug.Log("testing");
        string setnence = "PTEN, a putative protein tyrosine phosphatase gene mutated in human brain, breast, and prostate cancer.	Mapping of homozygous deletions on human chromosome 10q23 has led to the isolation of a candidate tumor suppressor gene , PTEN , that appears to be mutated at considerable frequency in human cancers . In preliminary screens , mutations of PTEN were detected in 31 % ( 13 / 42 ) of glioblastoma cell lines and xenografts , 100 % ( 4 / 4 ) of prostate cancer cell lines , 6 % ( 4 / 65 ) of breast cancer cell lines and xenografts , and 17 % ( 3 / 18 ) of STARTCODE001primary glioblastomasENDCODE002 . The predicted PTEN product has a protein tyrosine phosphatase domain and extensive homology to tensin , a protein that interacts with actin filaments at focal adhesions . These homologies suggest that PTEN may suppress tumor cell growth by antagonizing protein tyrosine kinases and may regulate tumor cell invasion and metastasis through interactions at focal adhesions . . ";
        ParseString(setnence);
        int[] locs = GetLoc();
        foreach(int i in locs)
        {
            Debug.Log(i);
        }

        ParseTagLoc(setnence);
        locs = GetLoc();
        foreach (int i in locs)
        {
            Debug.Log(i);
        }

    }

}
