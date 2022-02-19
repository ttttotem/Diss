using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    public int damage = 1;

    public AudioSource audio;

    public void Start()
    {
        StartCoroutine(Sound());
    }

    IEnumerator Sound() {
        yield return new WaitForSeconds(0.3f);
        audio.Play();
    }

    private void OnDestroy()
    {
        audio.Stop();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
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

