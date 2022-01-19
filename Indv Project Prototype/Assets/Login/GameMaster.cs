using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster GM;

    public void Awake()
    {
        if (GameMaster.GM == null)
        {
            GameMaster.GM = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    PlayerLogin.PL.SetStats();
        //}
    }
}
