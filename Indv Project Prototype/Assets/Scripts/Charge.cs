using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    public int damage = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            SmallUnit smallUnit = collision.gameObject.GetComponent<SmallUnit>();
            smallUnit.takeDamage(damage);
        }
    }
}
