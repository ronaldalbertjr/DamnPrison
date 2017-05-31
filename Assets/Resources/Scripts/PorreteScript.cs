using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorreteScript : MonoBehaviour
{
    [SerializeField]
    AnimationClip attackingAnimClip;
    [SerializeField]
    AnimationClip hittenAnimClip;
    [SerializeField]
    AnimationClip shieldFallingClip;
    [SerializeField]
    AnimationClip risingClip;
    [SerializeField]
    GameObject shieldPrefab;
    GameObject player;
    GameObject hitArea;
    BoxCollider2D hitAreaCollider;
    PorreteAreaHitScript hitAreaScript;
    SpriteRenderer spRenderer;
    Animator anim;
    float health;
    bool playerInsideArea;
    bool attacking;
    bool facingRight;
    bool damaged;
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
        damaged = false;
    }
    private void Start()
    {
        health = 10;
    }
    void Update ()
    {
        Flip();
        playerInsideArea = hitAreaScript.playerInsideArea;
        if(!playerInsideArea && !attacking && !damaged)
        {
            GetComponent<PolyNavAgent>().SetDestination(player.transform.position);
        }
        else if(!attacking && !damaged)
        {
            GetComponent<PolyNavAgent>().Stop();
            StartCoroutine(PorreteAttacking());
        }
        else if(damaged)
        {
            GetComponent<PolyNavAgent>().Stop();
        }
        if(health <= 0)
        {
            Destroy(gameObject);
            Time.timeScale = 1;
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

    void Damaged()
    {
        health--;
        if (health != 5)
        {
            StartCoroutine(IDamaged());
        }
        else if(health == 5)
        {
            StartCoroutine(IFatalDamaged());
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Bullet"))
        {
            Damaged();
            Destroy(col.gameObject);
        }
    }

    IEnumerator IDamaged()
    {
        damaged = true;
        StartCoroutine(GoingBackWhenShot());
        StartCoroutine(ChangeEnemyColor());
        StartCoroutine(ChangeTimeScale());
        if(health > 5) anim.SetTrigger("TakenDamage");
        if(health < 5) anim.SetTrigger("TakenFatalDamage");
        yield return new WaitForSecondsRealtime(hittenAnimClip.length);
        damaged = false;
    }

    IEnumerator IFatalDamaged()
    {
        damaged = true;
        StartCoroutine(GoingBackWhenShot());
        StartCoroutine(ChangeEnemyColor());
        StartCoroutine(ChangeTimeScale());
        anim.SetTrigger("Rising");
        anim.SetTrigger("TakenFatalDamage");
        GameObject shield = (GameObject) Instantiate(shieldPrefab, transform);
        anim.SetFloat("WithShield", 1);
        yield return new WaitForSecondsRealtime(shieldFallingClip.length);
        Destroy(shield);
        GetComponent<PolyNavAgent>().maxSpeed = 5f;
        yield return new WaitForSecondsRealtime(risingClip.length/2 - 0.5f);
        damaged = false;
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

    IEnumerator GoingBackWhenShot()
    {
        float multiplier = 0;
        Vector3 goingDirection = player.transform.position - transform.position;
        if(health > 5) multiplier = 0.05f;
        else if(health <= 5) multiplier = 0.08f;
        Vector3 goBackVector = new Vector3(-goingDirection.normalized.x, -goingDirection.normalized.y) * multiplier;
        for (float i = 0; i < 10; i++)
        {
            transform.position += goBackVector;
            yield return new WaitForSecondsRealtime(0.001f);
        }
    }

    IEnumerator ChangeEnemyColor()
    {
        spRenderer.color = new Color(0.3f, 0, 0);
        yield return new WaitForSecondsRealtime(0.05f);
        spRenderer.color = new Color(1, 1, 1);
    }

    IEnumerator ChangeTimeScale()
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(0.03f); ;
        Time.timeScale = 1;
    }


}
