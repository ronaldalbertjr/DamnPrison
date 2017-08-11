using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MuscleliniAttacks
{
    JumpAttack,
    PunchAttack
}

public class MuscleliniScript : MonoBehaviour
{
    #region Variables
        [SerializeField]
        AnimationClip punchAnimClip;
        [SerializeField]
        AnimationClip jumpAnimClip;

        Animator anim;
        GameObject player;
        SpriteRenderer spRenderer;

        MuscleliniAttacks currentAttack;

        float timeToAttack = 0;
        bool isDead;
        bool punching;
        bool playerInsideArea;
        bool facingRight;
        bool jumping;
        bool punchingCoroutineRunning;
        bool jumpingCoroutineRunning;
    #endregion

    void Start ()
    {
        anim = GetComponent<Animator>();
        spRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentAttack = MuscleliniAttacks.PunchAttack;
        punching = false;
        punchingCoroutineRunning = false;
        jumpingCoroutineRunning = false;
        isDead = false;
	}
	
	void Update ()
    {
        if (!isDead)
        {
            Flip(player.transform, spRenderer);
            if(!jumpingCoroutineRunning) timeToAttack += Time.deltaTime;
            if (timeToAttack >= 10)
            {
                timeToAttack = 0;
                currentAttack = MuscleliniAttacks.JumpAttack;
            }
            CheckAttack(currentAttack);
        }
	}

    void CheckAttack(MuscleliniAttacks currentAttack)
    {
        switch (currentAttack)
        {
            case MuscleliniAttacks.PunchAttack:
                if (punching)
                {
                    if (!punchingCoroutineRunning) StartCoroutine(Punching());
                }
                else
                {
                    anim.SetBool("Running", true);
                    GetComponent<PolyNavAgent>().SetDestination(player.transform.position);
                }
                break;
            case MuscleliniAttacks.JumpAttack:
                if (!jumpingCoroutineRunning)
                {
                    StartCoroutine(Jumping());
                }
                break;
        }
    }

    public void Flip(Transform toLookAt, SpriteRenderer spRenderer)
    {
        if (toLookAt.position.x > transform.position.x && !facingRight)
        {
            facingRight = true;
            ChangeScale(facingRight, spRenderer);
        }
        else if (toLookAt.position.x < transform.position.x && facingRight)
        {
            facingRight = false;
            ChangeScale(facingRight, spRenderer);
        }
    }

    void ChangeScale(bool flip, SpriteRenderer spRenderer)
    {
        spRenderer.flipX = flip;
    }

    IEnumerator Punching()
    {
        punchingCoroutineRunning = true;
        GetComponent<PolyNavAgent>().Stop();
        anim.SetTrigger("Punch");
        yield return new WaitForSecondsRealtime(0.5f);
        if (playerInsideArea)
        {
            player.GetComponent<PlayerBehaviour>().Damaged(gameObject);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        punching = false;
        punchingCoroutineRunning = false;
    }

    IEnumerator Jumping()
    {
        jumpingCoroutineRunning = true;
        anim.SetTrigger("Jump");
        yield return new WaitForSecondsRealtime(jumpAnimClip.length);
        transform.position += new Vector3(0, 9999999999999);
        jumpingCoroutineRunning = false;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag.Equals("Player") && currentAttack == MuscleliniAttacks.PunchAttack)
        {
            punching = true;
            GetComponent<PolyNavAgent>().Stop();
        }

        if(col.tag.Equals("Player"))
        {
            playerInsideArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag.Equals("Player"))
        {
            playerInsideArea = false;
        }
    }
}
