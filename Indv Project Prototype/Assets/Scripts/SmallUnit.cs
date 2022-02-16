using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallUnit : MonoBehaviour
{
    public int health = 100;

    public bool friendly = false;

    Money money;

    AudioManager audioManager;

    void Start()
    {
        money = FindObjectOfType<Money>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if(friendly == false)
            {
                money.AddMoney(50);
                if(audioManager != null)
                {
                    audioManager.Play("enemyDeath");
                }
            }
            else
            {
                if (audioManager != null)
                {
                    audioManager.Play("friendDeath");
                }
            }
            Destroy(gameObject);
        }
    }
}
