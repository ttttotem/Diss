using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capacitor : MonoBehaviour
{
    public Upgrades[] upgrades;

    public float chargeRange = 8f;
    public float range = 4f;
    public float chargeTime = 1f;
    public int damage = 100;

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

    public void Upgrade(int i)
    {
        if (i == 0)
        {
            IncreaseDamage();
        }
        else if (i == 1)
        {
            IncreaseRange();
        }
    }

    void IncreaseDamage()
    {
        if (damage >= upgrades[0].maxValue)
        {
            upgrades[0].available = false;
        }
        if (upgrades[0].available == false)
        {
            return;
        }
        damage += upgrades[0].amount;
    }

    void IncreaseRange()
    {
        if (range >= upgrades[1].maxValue)
        {
            upgrades[1].available = false;
        }
        if (upgrades[1].available == false)
        {
            return;
        }
        range += upgrades[1].amount;
    }
}
