using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorreteScript : MonoBehaviour
{
    #region Variables
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
        [SerializeField]
        AudioSource porreteAttackAudio;
        GameObject player;
        GameObject hitArea;
        BoxCollider2D hitAreaCollider;
        PorreteAreaHitScript hitAreaScript;
        [HideInInspector]
        public BoxColliderTriggerScript boxColliderTrigger;
        SpriteRenderer spRenderer;
        Animator anim;
        float health;
        bool playerInsideArea;
        bool attacking;
        bool facingRight;
        [HideInInspector]
        public bool duringHit;
        bool damaged;
    #endregion

    void OnEnable()
    {
        boxColliderTrigger = transform.parent.FindChild("BoxColliderTrigger").GetComponent<BoxColliderTriggerScript>();
        health = 10;
        player = GameObject.FindGameObjectWithTag("Player");
        hitArea = transform.FindChild("PorreteAttackArea").gameObject;
        hitAreaScript = hitArea.GetComponent<PorreteAreaHitScript>();
        hitAreaCollider = hitArea.GetComponent<BoxCollider2D>();
        spRenderer = transform.FindChild("PorreteSpriteRender").gameObject.GetComponent<SpriteRenderer>();
        anim = spRenderer.gameObject.GetComponent<Animator>();
        attacking = false;
        playerInsideArea = false;
        damaged = false;
        duringHit = false;
        Flip(player.transform, spRenderer, hitAreaCollider);
    }

    void Update ()
    {
        Flip(player.transform, spRenderer, hitAreaCollider);
        playerInsideArea = hitAreaScript.playerInsideArea;
        if(!playerInsideArea && !attacking && !damaged)
        {
            GetComponent<PolyNavAgent>().SetDestination(player.transform.position);
        }
        else if(!attacking && !damaged)
        {
            GetComponent<PolyNavAgent>().Stop();
            porreteAttackAudio.Play();
            StartCoroutine(PorreteAttacking());
        }
        else if(damaged)
        {
            GetComponent<PolyNavAgent>().Stop();
        }
	}

    public void Flip(Transform toLookAt, SpriteRenderer spRenderer, BoxCollider2D hitAreaCollider)
    {
        if (toLookAt.position.x > transform.position.x && !facingRight)
        {
            facingRight = true;
            ChangeScale(facingRight, spRenderer, hitAreaCollider);
        }
        else if (toLookAt.position.x < transform.position.x && facingRight)
        {
            facingRight = false;
            ChangeScale(facingRight, spRenderer, hitAreaCollider);
        }
    }

    void ChangeScale(bool flip, SpriteRenderer spRenderer, BoxCollider2D hitAreaCollider)
    {
        spRenderer.flipX = flip;
        hitAreaCollider.offset = new Vector2(-hitAreaCollider.offset.x, hitAreaCollider.offset.y);
    }

    public void Damaged(GameObject tank = null, bool hittenByTank = false)
    {
        health--;
        if (health != 5)
        {
            StartCoroutine(IDamaged(hittenByTank, tank));
        }
        else if(health == 5)
        {
            StartCoroutine(IFatalDamaged(hittenByTank, tank));
        }
        if (health <= 0)
        {
            Destroy(gameObject);
            Time.timeScale = 1;
            boxColliderTrigger.numberOfEnemiesInRoom--;
        }
    }

    IEnumerator IDamaged(bool hittenByTank, GameObject tank)
    {
        damaged = true;
        if (hittenByTank)
            StartCoroutine(HittenByTank(tank));
        else
            StartCoroutine(GoingBackWhenShot());
        StartCoroutine(ChangeEnemyColor());
        StartCoroutine(ChangeTimeScale());
        if(health > 5) anim.SetTrigger("TakenDamage");
        if(health < 5) anim.SetTrigger("TakenFatalDamage");
        yield return new WaitForSecondsRealtime(hittenAnimClip.length);
        damaged = false;
    }

    IEnumerator IFatalDamaged(bool hittenByTank, GameObject tank)
    {
        damaged = true;
        if (hittenByTank)
            StartCoroutine(HittenByTank(tank));
        else
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

    IEnumerator HittenByTank(GameObject tank)
    {
        duringHit = true;
        Vector3 auxVector = new Vector3(0, 0);
        if (tank.transform.position.x < transform.position.x)
        {
            if (tank.transform.position.y < transform.position.y)
            {
                auxVector = new Vector3(1F, 1F);
            }
            else if (tank.transform.position.y > transform.position.y)
            {
                auxVector = new Vector3(1F, -1F);
            }
        }
        else if (tank.transform.position.x > transform.position.x)
        {
            if (tank.transform.position.y < transform.position.y)
            {
                auxVector = new Vector3(-1F, 1F);
            }
            else if (tank.transform.position.y > transform.position.y)
            {
                auxVector = new Vector3(-1F, -1F);
            }
        }
        Vector3 diffVector = tank.GetComponent<TankScript>().differenceVector.normalized;
        Vector3 toSum = diffVector + auxVector;
        for (int i = 0; i < 10; i++)
        {
            transform.position += toSum * 0.25f;
            yield return new WaitForSeconds(0.001f);
        }
        anim.SetBool("Shot", false);
        yield return new WaitForSeconds(1f);
        duringHit = false;
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
        float multiplier = 0;;
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

    IEnumerator ChangeWalkingDirection(GameObject enemy)
    {
        Vector3 differenceVector = transform.position - enemy.transform.position;
        if (Mathf.Abs(differenceVector.x) < Mathf.Abs(differenceVector.y))
        {
            if (transform.position.x < enemy.transform.position.x)
            {
                for (int i = 0; i < 10; i++)
                {
                    transform.position += new Vector3(-1, 0) * 0.005f;
                    yield return new WaitForSeconds(0.001f);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    transform.position += new Vector3(1, 0) * 0.005f;
                    yield return new WaitForSeconds(0.001f);
                }
            }
        }
        else
        {
            if (transform.position.y < enemy.transform.position.y)
            {
                for (int i = 0; i < 10; i++)
                {
                    transform.position += new Vector3(0, -1) * 0.005f;
                    yield return new WaitForSeconds(0.001f);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    transform.position += new Vector3(0, 1) * 0.005f;
                    yield return new WaitForSeconds(0.001f);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Bullet") && GetComponent<PorreteScript>().isActiveAndEnabled)
        {
            Damaged();
            Destroy(col.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag.Equals("Untouchable") && this.isActiveAndEnabled)
        {
            StartCoroutine(ChangeWalkingDirection(col.gameObject));
        }
    }


}
