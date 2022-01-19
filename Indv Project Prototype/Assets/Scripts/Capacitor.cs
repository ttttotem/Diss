using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capacitor : MonoBehaviour
{
    public Upgrades[] upgrades;

    public float chargeRange = 8f;
    public float range = 4f;
    public float chargeRate = 1f;
    private float chargeCountdown = 0f;

    private Transform target;
    public string enemyTag = "Enemy";
    public GameObject chargePrefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance < chargeRange)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        var dir = target.position - transform.position;
        if (chargeCountdown <= 0f)
        {
            Shoot();
            chargeCountdown = 1f / chargeRate;
        }
        chargeCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject chargeGO = (GameObject)Instantiate(chargePrefab, transform.position, transform.rotation);
        Charge charge = chargeGO.GetComponent<Charge>();
        Destroy(chargeGO,0.1f);
    }

    public void Upgrade(int i)
    {
        if (i == 0)
        {
            IncreaseChargeSpeed();
        }
        else if (i == 1)
        {
            IncreaseRange();
        }
    }

    void IncreaseChargeSpeed()
    {
        if (chargeRate >= upgrades[0].maxValue)
        {
            upgrades[0].available = false;
        }
        if (upgrades[0].available == false)
        {
            return;
        }
        chargeRate += upgrades[0].amount;
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
