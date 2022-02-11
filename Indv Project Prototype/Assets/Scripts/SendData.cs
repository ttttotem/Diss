using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SendData : MonoBehaviour
{
    //inspect sent link to find
    string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfdeoB3cj0na1S9brlyajSTy2L-RBR39hZB7YJR3VO0F67AyA/formResponse";
    int counter = 0;
    string _sentence = "";
    public Points points;
    public LevelTracker levelTracker;

    public void addSentence(string sentence)
    {

        _sentence += "  \n \n " + sentence;

        //every 10 sentences send batch sentence
        if(counter == 10)
        {
            Send("user1", _sentence);
            counter = 0;
            _sentence = "";
            if(points != null)
            {
                points.SubmitScore();
            }
            
        }
        if(levelTracker != null)
        {
            levelTracker.IncreaseSubmission();
        }
        counter++;
    }

    public void Send(string user, string sentences)
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
        //User id
        form.AddField("entry.1188352957", user);

        //Sentences
        form.AddField("entry.1594666246", sentences);

        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

    }
}
