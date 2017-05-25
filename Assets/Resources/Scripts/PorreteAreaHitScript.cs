using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorreteAreaHitScript : MonoBehaviour
{
    [HideInInspector]
    public bool playerInsideArea;
    GameObject player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.Equals(player))
        {
            playerInsideArea = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.Equals(player))
        {
            playerInsideArea = false;
        }
    }
}
