using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmaScript : MonoBehaviour
{
    Transform pointPosition;
    Vector3 lookPosition;
    Quaternion rotation;
    Rigidbody2D body;
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        pointPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void FixedUpdate()
    {
        float AngleRad = Mathf.Atan2(-pointPosition.position.x + transform.position.x, pointPosition.position.y - transform.position.y);
        float angle = (180 / Mathf.PI) * AngleRad;
        this.body.rotation = angle;
    }
}
