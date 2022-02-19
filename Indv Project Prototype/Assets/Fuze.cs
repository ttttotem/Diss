using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuze : MonoBehaviour
{

    public float fireRate = 1f;
    private float fireCountdown = 0f;

    public int damage = 1;

    private Transform target;
    public float range = 15f;
    public float rotSpeed = 10f;

    public Transform firePoint;
    public GameObject bulletPrefab;

    public string enemyTag = "Enemy";

    AudioManager audioManager;

    public Transform turretTop;
    GameObject fire;

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

        foreach (GameObject enemy in enemies)
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
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 90;
        turretTop.rotation = Quaternion.Lerp(turretTop.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotSpeed);
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        if (fire != null)
        {
            return;
        }

        //Play burn sound
        if (audioManager != null)
        {
            audioManager.Play("Fire");
        }

        fire = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Fire damageBox = fire.GetComponentInChildren<Fire>();
        if(damageBox != null)
        {
            damageBox.damage = damage;
        }
        fire.transform.SetParent(turretTop.transform);
        Destroy(fire, 3f);
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
        else if (u.name == "Fire Rate")
        {
            IncreaseFireRate(u);
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
}
