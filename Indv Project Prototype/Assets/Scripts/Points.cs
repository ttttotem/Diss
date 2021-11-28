using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    [SerializeField]
    int[] correct_loc;
    int points = 0;
    public Text points_text;
    int bomb_loc = -1;
    
    public void set_Correct_Loc(int[] loc)
    {
        this.correct_loc = loc;
    }

    public void Set_Bomb_Loc(int loc)
    {
        bomb_loc = loc;
    }

    public void check_Loc(int[] loc)
    {
        if (correct_loc == null)
        {
            return;
        }

        //Bombs
        bool bomb_found = false;
        if(bomb_loc == -1)
        {
            bomb_found = true;
        } else if (loc.Contains(bomb_loc))
        {
            bomb_found = true;
        }
        if(bomb_found == false)
        {
            Bomb_Not_Found();
        }

        //Points for words found
        foreach(int i in loc)
        {
            if (correct_loc.Contains(i))
            {
                points += 1;
                points_text.text = "Points: " + points;
                Debug.Log("Correct");
            }
        }
    }

    public void Bomb_Not_Found()
    {
        Debug.Log("Bomb not found");
    }
}
