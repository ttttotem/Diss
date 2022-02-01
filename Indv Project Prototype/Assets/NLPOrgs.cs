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

public class NLPOrgs : MonoBehaviour
{
    [StreamingAssetFolderPath] public string nameFinderModel;
    public string[] models = new string[]
    {"organization"};

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
        string[] tokens = s.Split(' ');
        string superstring = "";
        int i = 0;
        foreach (string t in tokens)
        {
            i++;
            if (t.Equals("<START>"))
            {
                loc.Add(i);   
            } else if (t.Equals("<END>"))
            {
                loc.Add(i);
            } else
            {
                superstring += t;
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
