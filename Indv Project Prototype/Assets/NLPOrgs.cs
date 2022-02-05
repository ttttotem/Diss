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

public class NLPOrgs : MonoBehaviour
{
    [StreamingAssetFolderPath] public string nameFinderModel;
    public string[] models = new string[]
    {"organization"};

    public TextMeshProUGUI text_helper;

    List<int> loc = new List<int>();

    private EnglishNameFinder nameFinder;

    public void Start()
    {
        nameFinder = new EnglishNameFinder(FileUtilx.GetStreamingAssetFilePath(nameFinderModel));
    }

    public string ParseSentence(string s)
    {
        string string_with_tags = nameFinder.GetNames(models, s);
        return ParseTagLoc(string_with_tags);
    }

    public string ParseTagLoc(string s)
    {
        text_helper.text = s;
        text_helper.ForceMeshUpdate(true);
        string superstring = "";
        int i = 0;
        bool chaining = false;
        foreach (TMP_WordInfo wInfo in text_helper.textInfo.wordInfo)
        {
            if(wInfo.characterCount > 0)
            {
                string t = wInfo.GetWord();
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
            return new int[0];
        }
        return loc.ToArray();
    }

}
