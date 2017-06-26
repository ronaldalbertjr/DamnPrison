using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderTriggerScript : MonoBehaviour
{
    [SerializeField]
    GameObject[] roomColliders;
    public int maxPoints;
    public GameObject[] enemies;
    [SerializeField]
    Transform[] placesToSpawn;
    public GameObject door;
    GameObject cameraGameObject;
    [HideInInspector]
    public float numberOfEnemiesInRoom;
    [HideInInspector]
    public int pointsInTheRoom;
    [HideInInspector]
    public bool thereWereEnemiesInTheRoom;
    private void Start()
    {
        cameraGameObject = Camera.main.gameObject;
    }
    private void Update()
    {
        if (numberOfEnemiesInRoom <= 0 && thereWereEnemiesInTheRoom)
        {
            thereWereEnemiesInTheRoom = false;
            if (door.GetComponent<DoorScript>().canOpen)
                StartCoroutine(Camera.main.gameObject.GetComponent<CameraScript>().OpeningDoor(door));

        }

    }
    private void OnTriggerEnter2D(Collider2D col)
    {  
        if(col.tag.Equals("Player"))
        {
            if (numberOfEnemiesInRoom > 0)
            {
                thereWereEnemiesInTheRoom = true;
                col.GetComponent<PlayerBehaviour>().playerCanMove = false;
                StartCoroutine(cameraGameObject.GetComponent<CameraScript>().MovingCamera(door, roomColliders));
            }
            else
            {
                thereWereEnemiesInTheRoom = false;
            }
        }
    }
}
