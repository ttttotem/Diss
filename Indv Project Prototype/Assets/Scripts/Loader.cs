using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Loader : MonoBehaviour
{
    public TextAsset textFile;     // drop your file here in inspector
    Sentences sentences_json;
    Sentence[] sentences;
    int currentIndex = 0;
    public TextMeshProUGUI text;
    string[] correctWords;
    int bomb_loc = -1;
    //may have to use string instead of ints if tmp doesn't get the same values
    public int no_bomb_chance = 0; // 0 = 100% for bomb 1 = 0%
    public int max_text_length = 300;


    DiffcultyMod diff_mod;

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
        sentences_json = JsonUtility.FromJson<Sentences>(textFile.text);
        sentences = sentences_json.sentences;
    }

    public void Load_Next_Sentence()
    {
        diff_mod.Load_Sentence();
        if(currentIndex < sentences.Length)
        {
            string temp_text = sentences[currentIndex].text;
            //Cut to fit screen
            if(temp_text.Length > max_text_length)
            {
                temp_text = temp_text.Substring(0, max_text_length);
                //May need to adjust correct loc with CutTextToFit in future
            }
            if(Random.value > no_bomb_chance)
            {
                // %Chance for a bomb
                temp_text = PutBomb(temp_text);
                GameManager.GM.GetComponent<Points>().Set_Bomb_Loc(bomb_loc);
            }
            text.text = temp_text;
            GameManager.GM.GetComponent<Points>().set_Correct_Loc(sentences[currentIndex].pos_correct);
            currentIndex += 1;

            //start timer
            diff_mod.StartTimer();
        } else
        {
            text.text = "Out of sentences";
            GameManager.GM.GetComponent<Points>().set_Correct_Loc(null);
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
