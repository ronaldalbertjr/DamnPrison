using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    float speed;
    [SerializeField]
    Transform playerPosition;
    [SerializeField]
    GameObject door;
    [SerializeField]
    Transform[] cameraBorders;

    [HideInInspector]
    public bool lookingAtClosedDoor;

    void Update ()
    {
        if (playerPosition.position.y > cameraBorders[0].position.y || playerPosition.position.x < cameraBorders[1].position.x || playerPosition.position.x > cameraBorders[2].position.x || playerPosition.position.y < cameraBorders[3].position.y)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPosition.position.x + offset.x, playerPosition.position.y + offset.y, -14f), speed * Time.deltaTime);
        }
    }
}
