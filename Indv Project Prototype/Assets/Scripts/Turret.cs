using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public Upgrades[] upgrades;

    public float fireRate = 1f;
    private float fireCountdown = 0f;

    private Transform target;
    public float range = 15f;
    public float rotSpeed = 10f;

    public Transform firePoint;
    public GameObject bulletPrefab;

    public string enemyTag = "Enemy";

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
        
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance < range)
        {
            target = nearestEnemy.transform;
        } else
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
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotSpeed);
        if ( fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null)
        {
            bullet.Seek(target);
        }
    }

    public void Upgrade(int i)
    {
        if (i == 0)
        {
            IncreaseFireSpeed();
        } else if (i == 1)
        {
            IncreaseRange();
        }
    }

    void IncreaseFireSpeed()
    {
        if(fireRate >= upgrades[0].maxValue)
        {
            upgrades[0].available = false;
        }
        if (upgrades[0].available == false)
        {
            return;
        }
        fireRate += upgrades[0].amount;
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
