using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorreteAreaHitScript : MonoBehaviour
{
    #region Variables
        [HideInInspector]
        public bool playerInsideArea;
        GameObject player;
    #endregion

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.Equals(player) && col.GetComponent<PlayerBehaviour>().canBeHitten && !col.GetComponent<PlayerBehaviour>().dead)
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
