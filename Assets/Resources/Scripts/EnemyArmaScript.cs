using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmaScript : MonoBehaviour
{
    #region Variables
        [HideInInspector]
        public Transform pointPosition;
        Vector3 lookPosition;
        Quaternion rotation;
    #endregion

    void Awake()
    {
        pointPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float AngleRad = Mathf.Atan2(-pointPosition.position.x + transform.position.x, pointPosition.position.y - transform.position.y);
        float angle = (180 / Mathf.PI) * AngleRad;
        transform.rotation = Quaternion.Euler(0f,0f,angle);
    }
}
