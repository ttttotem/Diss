using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Loader : MonoBehaviour
{
    public TextAsset textFile;
    Sentences sentences_json;
    CustomSentence[] sentences;

    public TextAsset knownSentencesFile;

    int currentIndex = 0;
    public TextMeshProUGUI text;
    string[] correctWords;
    int bomb_loc = -1;
    //may have to use string instead of ints if tmp doesn't get the same values
    public int no_bomb_chance = 0; // 0 = 100% for bomb 1 = 0%
    public int max_text_length = 300;

    public PopUp popup;

    public NLPOrgs NLPOrgs;

    public int KnownSentenceChance = 1;

    int tutorialIndex = 0;

    DiffcultyMod diff_mod;

    public bool Sendable = false;

    string[] s;
    string[] knownSentences;

    Points points;

    char[] seperators = new[] { ' ', '/' }; //There may be more

    public void Set_Bomb_Chance(int i)
    {
        if(i >= 0 && i <= 1)
        {
            no_bomb_chance = i;

        } else
        {
            Debug.Log("Bomb chance not valid value");
            no_bomb_chance = 1;
        }
        
    }



    void Start()
    {
        diff_mod = GetComponent<DiffcultyMod>();
        s = textFile.text.Split('\n');
        if(knownSentencesFile != null)
        {
            knownSentences = knownSentencesFile.text.Split('\n');
        }
        points = GameManager.GM.GetComponent<Points>();
    }

    public void Load_Next_Sentence()
    {
        Sendable = false;

        if (GameManager.GM.tutorial == true)
        {
            points.knownSentence = true;
            Load_Tutorial_Sentence();
        }
        else if (Random.Range(0, KnownSentenceChance) == 0)
        {
            points.knownSentence = true;
            LoadKnownSentence();
        }
        else
        {
            points.knownSentence = false;
            Load_Unknown_Sentence();
        }
    }

    public void Load_Prev_Sentence()
    {
        Sendable = false;

        if (GameManager.GM.tutorial == true)
        {
            points.knownSentence = true;
            tutorialIndex -= 1;
            Load_Tutorial_Sentence();
        }
        else
        {
            points.knownSentence = true;
            currentIndex -= 1;
            LoadKnownSentence();
        }
    }

    public void Load_Unknown_Sentence()
    {
        
        diff_mod.Load_Sentence();
        if (currentIndex < s.Length)
        {
            string temp_text = s[currentIndex];
            //Cut to fit screen
            if (temp_text.Length > max_text_length)
            {
                temp_text = temp_text.Substring(0, max_text_length);
                //May need to adjust correct loc with CutTextToFit in future
            }
            if (Random.value > no_bomb_chance)
            {
                // %Chance for a bomb
                temp_text = PutBomb(temp_text);
                GameManager.GM.GetComponent<Points>().Set_Bomb_Loc(bomb_loc);
            }
            NLPOrgs.ParseSentence(temp_text);
            text.text = temp_text;
            GameManager.GM.GetComponent<Points>().set_Correct_Loc(NLPOrgs.GetLoc());
            NLPOrgs.ClearLoc();
            currentIndex += 1;

            //Flag this sentence as intresting
            Sendable = true;

            //start timer
            diff_mod.StartTimer();
        }
        else
        {
            text.text = "Out of sentences";
            GameManager.GM.GetComponent<Points>().set_Correct_Loc(null);
        }
    }

    public void Load_Tutorial_Sentence()
    {
        if (knownSentences.Length == 0 || knownSentences == null)
        {
            Load_Next_Sentence();
            return;
        }
        if (tutorialIndex >= knownSentences.Length)
        {
            popup.SetActive(true);
            return;
        } 
        string temp_text = knownSentences[tutorialIndex];
        string loc = "";
        if (temp_text.Length > 0)
        {
            int i = temp_text.IndexOf(" ");

            //Split the locations from the setence
            loc = temp_text.Substring(0, i);
            temp_text = temp_text.Substring(i + 1);
            string[] temp = loc.Split(',');

            //Turn the locations into an array
            int[] locations = new int[temp.Length];
            for (int j = 0; j < temp.Length; j++)
            {
                int val = 0;
                if (int.TryParse(temp[j], out val))
                {
                    locations[j] = val;
                }
                else
                {
                    locations[j] = -2;
                }
            }
            text.text = temp_text;
            GameManager.GM.GetComponent<Points>().set_Correct_Loc(locations);

            tutorialIndex++;

            //start timer
            diff_mod.StartTimer();
        }
        else
        {
            Load_Next_Sentence();
            return;
        }
    }

    public void LoadKnownSentence()
    {
        if(knownSentences.Length == 0 || knownSentences == null)
        {
            Load_Next_Sentence();
            return;
        }
        currentIndex = Random.Range(0, knownSentences.Length);
        string temp_text = knownSentences[currentIndex];
        string loc = "";
        if(temp_text.Length > 0)
        {
            int i = temp_text.IndexOf(" ");

            //Split the locations from the setence
            loc = temp_text.Substring(0, i);
            temp_text = temp_text.Substring(i+1);
            string[] temp = loc.Split(',');

            //Turn the locations into an array
            int[] locations = new int[temp.Length];
            for(int j = 0; j<temp.Length; j++)
            {
                int val = 0;
                if(int.TryParse(temp[j], out val))
                {
                    locations[j] = val;
                }
                else
                {
                    locations[j] = -2;
                }
            }
            text.text = temp_text;
            GameManager.GM.GetComponent<Points>().set_Correct_Loc(locations);

            //start timer
            diff_mod.StartTimer();
        } else
        {
            Load_Next_Sentence();
            return;
        }
    }

    /* May need this later
    public string CutTextToFit(string s)
    {
        string return_string;
        return_string = s.Substring(0, max_text_length);
        string[] split = return_string.Split(seperators);
        List<string> split_list = new List<string>(split);
        for (int i = 0; i < sentences[currentIndex].pos_correct.Length; i++)
        {
            if (sentences[currentIndex].pos_correct[i] >= split.Length)
            {
                sentences[currentIndex].pos_correct[i] += 1;
            }
        }
    }
    */

    public string PutBomb(string s)
    {
        string[] split = s.Split(seperators);
        List<string> split_list = new List<string>(split);
        bomb_loc = Random.Range(0, split.Length);
        split_list.Insert(bomb_loc, "BOMB");

        //Adjust pos_correct to match with new word
        //Wont work till you fix sentences
        for (int i = 0; i < sentences[currentIndex].pos_correct.Length; i++)
        {
            if(sentences[currentIndex].pos_correct[i] >= bomb_loc)
            {
                sentences[currentIndex].pos_correct[i] += 1;
            }
        }
        return string.Join(" ", split_list.ToArray());
    }
}
