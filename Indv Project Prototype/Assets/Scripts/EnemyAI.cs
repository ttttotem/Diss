using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public float speed = 10f;

    private Transform target;
    private int wavepointIndex = 0;

    public float rotSpeed = 10f;

    public float animDuration = 0.04f;
    float animCurrentTime = 0;

    public float animTimer = 1f;
    float currentTime = 0;

    public Transform graphics;

    Animator animator;

    //public GameObject graphics;

    // Start is called before the first frame update
    void Start()
    {
        target = Waypoints.points[0];
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime < 0)
        {
            if (animCurrentTime < 0)
            {
                //Done waiting
                animCurrentTime = animDuration;
                currentTime = animTimer;
                if(animator != null)
                {
                    animator.SetTrigger("Move");
                }
            }
            else
            {
                animCurrentTime -= Time.deltaTime;
                return;
            }
        }

        currentTime -= Time.deltaTime;

        Vector2 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(graphics != null)
        {
            graphics.rotation = Quaternion.Lerp(graphics.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotSpeed);
        }
        
        if (Vector2.Distance(transform.position, target.position) <= 0.1f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if(wavepointIndex >= Waypoints.points.Length - 1)
        {
            Destroy(gameObject);
            EnemyEnd.instance.LoseLives(1);
            return;
        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }
}
