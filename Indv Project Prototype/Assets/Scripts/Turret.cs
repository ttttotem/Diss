using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    private Transform target;
    public float range = 15f;
    public float rotSpeed = 10f;

    public Transform firePoint;
    public GameObject bulletPrefab;

    public string enemyTag = "Enemy";

    AudioManager audioManager;

    public Transform turretTop;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        audioManager = FindObjectOfType<AudioManager>();
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
        angle -= 90;
        turretTop.rotation = Quaternion.Lerp(turretTop.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotSpeed);
        if ( fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }
    void Shoot()
    {
        //Play shoot sound
        if(audioManager != null)
        {
            audioManager.Play("laser2");
        }

        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null)
        {
            bullet.Seek(target);
        }
    }

    public void Upgrade(Upgrades u)
    {
        if (u.available == false)
        {
            return;
        }

        if (u.name == "Fire Rate")
        {
            IncreaseFireRate(u);
        }
        else if (u.name == "Range")
        {
            IncreaseRange(u);
        }
    }

    void IncreaseFireRate(Upgrades u)
    {
        fireRate += u.amount;
        if (fireRate >= u.maxValue)
        {
            fireRate = u.maxValue;
            u.available = false;
            return;
        }
    }

    void IncreaseRange(Upgrades u)
    {
        range += u.amount;
        if (range >= u.maxValue)
        {
            range = u.maxValue;
            u.available = false;
            return;
        }
    }

}
