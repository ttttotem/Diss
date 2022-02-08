using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyEnd : MonoBehaviour
{

    public static AllyEnd instance;

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
    }
    
    public void SetRequired(int val)
    {
        if(val >= 0)
        {
            required = val;
            text.text = safe.ToString() + "/" + required.ToString();
        }
    }

    public void OneBackSafe()
    {
        safe += 1;
        text.text = safe.ToString() + "/" + required.ToString();
    }
}
