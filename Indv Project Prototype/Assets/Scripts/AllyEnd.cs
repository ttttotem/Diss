using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyEnd : MonoBehaviour
{

    public static AllyEnd instance;

    AudioManager am;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one build manager in scene");
            return;
        }
        instance = this;
    }

    public int required=6;
    private int safe = 0;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = safe.ToString() + "/" + required.ToString();
        am = FindObjectOfType<AudioManager>();
    }
    
    public void SetRequired(int val)
    {
        safe = 0;
        if(val >= 0)
        {
            required = val;
            text.text = safe.ToString() + "/" + required.ToString();
        }
    }

    public bool CheckPassed()
    {
        if(required <= safe)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OneBackSafe()
    {
        if (am != null)
        {
            am.Play("shortBuzz");
        }

        safe += 1;
        text.text = safe.ToString() + "/" + required.ToString();
    }
}
