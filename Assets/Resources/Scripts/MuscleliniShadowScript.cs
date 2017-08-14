using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleliniShadowScript : MonoBehaviour
{
    #region Variables
        GameObject player;
        MuscleliniScript musclelini;
        bool shadowFollowing;
        float timeToFall;
    #endregion

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        musclelini = GameObject.FindGameObjectWithTag("Boss").GetComponent<MuscleliniScript>();
        GetComponent<PolyNavAgent>().OnDestinationReached += GetTheTankToFall;
        shadowFollowing = true;
    }

    private void Update()
    {
        if (shadowFollowing)
        {
            GetComponent<PolyNavAgent>().SetDestination(player.transform.position + new Vector3(0, -1.5f));
            timeToFall += Time.deltaTime;
            if(timeToFall >= 5)
            {
                GetTheTankToFall();
                shadowFollowing = false;
            }
        }
    }

    private void GetTheTankToFall()
    {
        musclelini.StartFalling(transform.position);
        shadowFollowing = false;
    }
}
