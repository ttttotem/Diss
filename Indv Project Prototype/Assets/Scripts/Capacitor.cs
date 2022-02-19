using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capacitor : MonoBehaviour
{
    public float chargeTime = 1f;
    public int damage = 100;
    public float tempChargeTime = 1f;

    public GameObject chargePrefab;

    public Animator anim;

    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void Shoot()
    {
        StartCoroutine(ChargeShot());
    }

    IEnumerator ChargeShot()
    {
        anim.SetBool("charging", true);
        //Play charge sound
        if (audioManager != null)
        {
            audioManager.Play("charging2");
        }

        yield return new WaitForSeconds(chargeTime);
        anim.SetBool("charging", false);

        //Play shoot sound
        if (audioManager != null)
        {
            audioManager.Play("zap");
        }
        GameObject chargeGO = (GameObject)Instantiate(chargePrefab, transform.position, transform.rotation);
        Charge charge = chargeGO.GetComponent<Charge>();
        charge.damage = damage;
        Destroy(chargeGO, 0.4f);
    }

    public void Upgrade(Upgrades u)
    {
        if (u.available == false)
        {
            return;
        }

        if (u.name == "Damage")
        {
            IncreaseDamage(u);
        }
        else if (u.name == "Charge")
        {
            IncreaseRange(u);
        }
    }

    void IncreaseDamage(Upgrades u)
    {
        damage += u.amount;
        if (damage >= u.maxValue)
        {
            damage = u.maxValue;
            u.available = false;
            return;
        }
    }

    void IncreaseRange(Upgrades u)
    {
        tempChargeTime += u.amount;
        if (tempChargeTime >= u.maxValue)
        {
            tempChargeTime = u.maxValue;
            u.available = false;
            return;
        }
    }
}
