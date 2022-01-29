using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformMovement : MonoBehaviour
{
    public Transform[] waypoints;
    private Transform target;
    public float speed;
    
    void Start()
    {
        target = waypoints[0];
    }

    void Update()
    {
        Vector2 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        if (Vector2.Distance(transform.position, target.position) < 0.3f)
        {
            target = target == waypoints[1] ? waypoints[0] : waypoints[1];
        }
    }
}
