using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyAI : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int wavepointIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        target = AllyWayPoints.points[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) <= 0.1f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= AllyWayPoints.points.Length - 1)
        {
            AllyEnd.instance.OneBackSafe();
            Destroy(gameObject);
            return;
        }

        wavepointIndex++;
        target = AllyWayPoints.points[wavepointIndex];
    }
}
