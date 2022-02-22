using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using PlayFab;

public class SendData : MonoBehaviour
{
    //inspect sent link to find
    string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfdeoB3cj0na1S9brlyajSTy2L-RBR39hZB7YJR3VO0F67AyA/formResponse";
    int counter = 0;
    string _sentence = "";
    public Points points;

    public void addSentence(string sentence)
    {

        _sentence += "  \n \n " + sentence;

        //every 1 sentences send batch sentence
        if(counter == 1)
        {
            Send(GameManager.GM.PlayerID, _sentence);
            counter = 0;
            _sentence = "";
        }
        counter++;
    }

    public void Send(string user, string sentences="")
    {
        StartCoroutine(Post(user,sentences));
    }

    IEnumerator Post(string user, string sentences)
    {
        if(user == null || user.Length == 0)
        {
            user = "Guest";
        }
        if(sentences == null || sentences.Length == 0)
        {
            Debug.Log("No sentences being sent");
            yield return null;
        }
        Debug.Log("Sending");
        WWWForm form = new WWWForm();

        //User id, Hidden score, and system, system times
        string system = "A";
        if (GameManager.GM.SystemA == false)
        {
            system = "B";
        }
        form.AddField("entry.1188352957","User: " + user + " Hidden taken: " + points.hiddenTaken + " Hidden Correct: " + points.hiddenCorrect + " System: " + system + " SystemATime: " + GameManager.GM.SysATime + " SystemBTime: " + GameManager.GM.SysBTime);

        //Sentences
        form.AddField("entry.1594666246", sentences);
        
        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

    }
}
