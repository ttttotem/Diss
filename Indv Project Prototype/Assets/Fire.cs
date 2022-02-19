using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    public int damage = 1;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.tag == "Enemy")
        {
            SmallUnit smallUnit = collision.gameObject.GetComponent<SmallUnit>();
            if (smallUnit != null)
            {
                smallUnit.takeDamage(damage);
            }
        }
    }
}

