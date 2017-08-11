using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleliniShadowScript : MonoBehaviour
{
    GameObject player;
    MuscleliniScript musclelini;
    bool shadowFollowing;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        musclelini = GameObject.FindGameObjectWithTag("Boss").GetComponent<MuscleliniScript>();
        GetComponent<PolyNavAgent>().OnDestinationReached += GetTheTankToFall;
        shadowFollowing = true;
    }

    private void Update()
    {
        if(shadowFollowing)
        GetComponent<PolyNavAgent>().SetDestination(player.transform.position + new Vector3(0, -1.5f));
    }

    private void GetTheTankToFall()
    {
        musclelini.StartFalling(transform.position);
        shadowFollowing = false;
    }
}
