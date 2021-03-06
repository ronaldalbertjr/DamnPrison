﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomColliderScript : MonoBehaviour
{
    #region Variables
        public List<GameObject> enemiesInRoom;
        bool hasBeenEntered;
    #endregion

    private void Start()
    {
        hasBeenEntered = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            RoomEntered();
        }
        else if(col.GetComponent<EnemyMovementDefaultScript>())
        {
            col.GetComponent<EnemyMovementDefaultScript>().roomCollider = this.gameObject;
        }
    }

    public void RoomEntered()
    {
        if (!hasBeenEntered)
        {
            hasBeenEntered = true;
            foreach (GameObject e in enemiesInRoom)
            {
                e.GetComponent<EnemyMovementDefaultScript>().enabled = false;
                if (e.GetComponent<RangedEnemyScript>())
                {
                    e.GetComponent<RangedEnemyScript>().enabled = true;
                }
                else if (e.GetComponent<DefaultEnemyScript>())
                {
                    e.GetComponent<DefaultEnemyScript>().enabled = true;
                }
                else if (e.GetComponent<TankScript>())
                {
                    StartCoroutine(SetTankScriptEnabled(e));
                }
                else if (e.GetComponent<PorreteScript>())
                {
                    e.GetComponent<PorreteScript>().enabled = true;
                }
                else if (e.GetComponent<DogScript>())
                {
                    e.GetComponent<DogScript>().enabled = true;
                }
            }
        }
    }

    IEnumerator SetTankScriptEnabled(GameObject e)
    {
        yield return new WaitForSecondsRealtime(1f);
        e.GetComponent<TankScript>().enabled = true;
    }
}
