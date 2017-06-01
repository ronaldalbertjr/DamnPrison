using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomColliderScript : MonoBehaviour
{
    public GameObject[] enemiesInRoom;
    bool hasBeenEntered;

    private void Start()
    {
        hasBeenEntered = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player") && !hasBeenEntered)
        {
            hasBeenEntered = true;
            foreach (GameObject e in enemiesInRoom)
            {
                e.GetComponent<EnemyMovementDefaultScript>().enabled = false;
                if(e.GetComponent<RangedEnemyScript>())
                {
                    e.GetComponent<RangedEnemyScript>().enabled = true;
                }
                else if(e.GetComponent<DefaultEnemyScript>())
                {
                    e.GetComponent<DefaultEnemyScript>().enabled = true;
                }
                else if(e.GetComponent<TankScript>())
                {
                    e.GetComponent<TankScript>().enabled = true;
                }
                else if(e.GetComponent<PorreteScript>())
                {
                    e.GetComponent<PorreteScript>().enabled = true;
                }
                else if(e.GetComponent<DogScript>())
                {
                    e.GetComponent<DogScript>().enabled = true;
                }
            }
        }
    }
}
