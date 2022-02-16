using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapacitorManager : MonoBehaviour
{
    public float chargeTime = 3f;
    private float chargeCountdown = 0f;

    public WaveUI waveUI;

    public void Update()
    {
        if(waveUI.WaveStarted == false)
        {
            return;
        }
        if (chargeCountdown <= 0f)
        {
            Capacitor[] capacitors = GetComponentsInChildren<Capacitor>();
            foreach(Capacitor capacitor in capacitors)
            {
                capacitor.Shoot();
            }
            chargeCountdown = chargeTime;
        }
        chargeCountdown -= Time.deltaTime;
    }
}
