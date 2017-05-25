using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorreteScript : MonoBehaviour
{
    [SerializeField]
    AnimationClip attackingAnimClip;
    GameObject player;
    GameObject hitArea;
    BoxCollider2D hitAreaCollider;
    PorreteAreaHitScript hitAreaScript;
    SpriteRenderer spRenderer;
    Animator anim;
    bool playerInsideArea;
    bool attacking;
    bool facingRight;
	void Awake ()
    {
		player = GameObject.FindGameObjectWithTag("Player");
        hitArea = transform.FindChild("PorreteAttackArea").gameObject;
        hitAreaScript = hitArea.GetComponent<PorreteAreaHitScript>();
        hitAreaCollider = hitArea.GetComponent<BoxCollider2D>();
        spRenderer = transform.FindChild("PorreteSpriteRender").gameObject.GetComponent<SpriteRenderer>();
        anim = spRenderer.gameObject.GetComponent<Animator>();
        attacking = false;
        playerInsideArea = false;
    }
	void Update ()
    {
        Flip();
        playerInsideArea = hitAreaScript.playerInsideArea;
        if(!playerInsideArea && !attacking)
        {
            GetComponent<PolyNavAgent>().SetDestination(player.transform.position);
        }
        else if(!attacking)
        {
            StartCoroutine(PorreteAttacking());
        }
	}

    void Flip()
    {
        if (player.transform.position.x > transform.position.x && !facingRight)
        {
            facingRight = true;
            ChangeScale(facingRight);
        }
        else if (player.transform.position.x < transform.position.x && facingRight)
        {
            facingRight = false;
            ChangeScale(facingRight);
        }
    }
    void ChangeScale(bool flip)
    {
        spRenderer.flipX = flip;
        hitAreaCollider.offset = new Vector2(-hitAreaCollider.offset.x, hitAreaCollider.offset.y);
    }
    IEnumerator PorreteAttacking()
    {
        attacking = true;
        anim.SetTrigger("Attacking");
        yield return new WaitForSecondsRealtime(attackingAnimClip.length/2);
        player.GetComponent<PlayerBehaviour>().Damaged(gameObject);
        yield return new WaitForSeconds(attackingAnimClip.length / 2);
        attacking = false;
    }
    

}
