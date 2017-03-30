using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    Transform playerPosition;
    [SerializeField]
    GameObject door;

    [HideInInspector]
    public bool lookingAtClosedDoor;

    void Update ()
    {
        if(lookingAtClosedDoor)
        {
                this.transform.position = Vector3.Lerp(transform.position, new Vector3(door.transform.position.x + offset.x, door.transform.position.y + offset.y, -14f), 5 * Time.deltaTime);
        }
        else
        {
                this.transform.position = Vector3.Lerp(transform.position, new Vector3(playerPosition.position.x + offset.x, playerPosition.position.y + offset.y, -14f), 5 * Time.deltaTime);
        }
    }
}
