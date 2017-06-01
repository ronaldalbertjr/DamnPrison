using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderTriggerScript : MonoBehaviour
{
    [SerializeField]
    GameObject[] enemies;
    [SerializeField]
    GameObject[] roomColliders;
    [SerializeField]
    GameObject door;
    GameObject cameraGameObject;
    private void Start()
    {
        cameraGameObject = Camera.main.gameObject;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {  
        if(col.tag.Equals("Player"))
        {
            col.GetComponent<PlayerBehaviour>().playerCanMove = false;
            StartCoroutine(cameraGameObject.GetComponent<CameraScript>().MovingCamera(enemies, door, roomColliders));
        }
    }
}
