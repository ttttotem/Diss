using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using LoginResult = PlayFab.ClientModels.LoginResult;


public class Points : MonoBehaviour
{
    [SerializeField]
    public int[] correct_loc;
    int points = 0;

    int hiddenPoints = 0;
    int hiddenTaken = 0;

    public Text points_text;
    int bomb_loc = -1;
    public bool knownSentence = false;

    public void Start()
    {
        if(points_text == null)
        {
            points_text = GameObject.Find("Points").GetComponent<Text>();
        }
    }

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

        int temp_points = 0;

        //Points for words found
        foreach(int i in loc)
        {
            if (correct_loc.Contains(i))
            {
                temp_points += 1;
                
            }
        }

        //Scale points by number of correct
        if(temp_points == 0.5 * correct_loc.Length || temp_points > 2)
        {
            temp_points = 1;
        } else
        {
            temp_points = 0;
        }

        //If guessing lots of words subtract points
        int extra_guesses = loc.Length - correct_loc.Length;
        if(extra_guesses > 5)
        {
            //User has made excessive guesses
            temp_points = 0;
        }

        points += temp_points;

        points_text.text = "Points: " + points;
    }

    public bool check_Hits(int[] loc)
    {
        //Count how many guesses where wrong
        hiddenTaken += 1;
        List<int> hits = new List<int>();
        foreach (int i in loc)
        {
            if (!correct_loc.Contains(i))
            {
                Debug.Log("Shouldnt hit " + i);
                hits.Add(i);
            }
        }
        if(hits.Count == 0)
        {
            hiddenPoints += 1;
            return true;
        }
        return false;
    }

    public bool check_Misses(int[] loc)
    {
        //Count how many correct answers were missed
        List<int> misses = new List<int>();
        foreach (int i in correct_loc)
        {
            if (!loc.Contains(i))
            {
                Debug.Log("Missed " + i);
                misses.Add(i);
            }
        }
        if (misses.Count == 0)
        {
            hiddenPoints += 1;
            return true;
        }
        return false;
    }

    public void Bomb_Not_Found()
    {
        Debug.Log("Bomb not found");
    }
        public void SubmitScore()
        {
            if (!PlayFabClientAPI.IsClientLoggedIn())
            {
                return;
            }
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = "Score",
                Value = points
            }
        }
            }, result => OnStatisticsUpdated(result), FailureCallback);
        }

        private void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult)
        {
            Debug.Log("Successfully submitted high score");
        }

        private void FailureCallback(PlayFabError error)
        {
            Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
            Debug.LogError(error.GenerateErrorReport());
        }
}
