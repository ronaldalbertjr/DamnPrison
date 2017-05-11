using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour
{
    [SerializeField]
    AnimationClip hitAnimClip;
    [SerializeField]
    float speed;
    GameObject player;
    Animator anim;
    Vector3 runDirection;
    Rigidbody2D rb;
    bool running;
	void Start ()
    {
        running = true;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        runDirection = player.transform.position - transform.position;
	}
	void Update ()
    {
		if(running)
        {
            anim.SetBool("Running", true);
            transform.position += runDirection.normalized * Time.deltaTime * speed;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Collision());
    }

    IEnumerator Collision()
    {
        anim.SetTrigger("Hit");
        anim.SetBool("Running", false);
        runDirection = player.transform.position - transform.position;
        running = false;
        yield return new WaitForSeconds(hitAnimClip.length);
        running = true;
        anim.SetBool("Running", true);
    }
}
