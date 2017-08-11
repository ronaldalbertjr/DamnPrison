using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleliniShadowScript : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        GetComponent<PolyNavAgent>().SetDestination(player.transform.position + new Vector3(0, -1.5f));
    }
}
