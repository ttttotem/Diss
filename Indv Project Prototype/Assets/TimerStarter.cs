using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStarter : MonoBehaviour
{
    public void StartTimer()
    {
        GameManager.GM.StartTimer();
    }
}
