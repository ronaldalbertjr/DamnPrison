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
        [SerializeField]
        GameObject muscleliniShadow;
        [SerializeField]
        BoxCollider2D boxCollider;

        Animator anim;
        GameObject player;
        GameObject inGameShadow;
        SpriteRenderer spRenderer;

        MuscleliniAttacks currentAttack;

        float timeToAttack = 0;
        public float health = 50;
        bool isDead;
        bool isAngry;
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
            if(!jumpingCoroutineRunning) Flip(player.transform, spRenderer);
            if(!jumpingCoroutineRunning) timeToAttack += Time.deltaTime;
            if (timeToAttack >= 10)
            {
                timeToAttack = 0;
                currentAttack = MuscleliniAttacks.JumpAttack;
            }
            CheckAttack(currentAttack);
            if(health <= 10)
            {
                spRenderer.color = new Color(255f, 0f, 0f);
                isAngry = true;
                GetComponent<PolyNavAgent>().maxSpeed = 20;
            }
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
                    anim.SetBool("Run", true);
                    GetComponent<PolyNavAgent>().SetDestination(player.transform.position);
                }
                break;
            case MuscleliniAttacks.JumpAttack:
                GetComponent<PolyNavAgent>().Stop();
                if (!jumpingCoroutineRunning)
                {
                    StartCoroutine(Jumping());
                }
                break;
        }
    }

    void CameraShake(float shakeAmount, float shakeDuration)
    {
        Vector3 camPosition = Camera.main.transform.position;
        if (shakeDuration > 0)
        {
            Camera.main.transform.localPosition = camPosition + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
            Camera.main.transform.localPosition = camPosition;
        }
    }

    void ChangeScale(bool flip, SpriteRenderer spRenderer)
    {
        spRenderer.flipX = flip;
    }

    void ChangeSpriteLayer(GameObject player)
    {
        if(player.transform.position.y < transform.position.y)
        {
            spRenderer.sortingOrder = -4;
        }
        else
        {
            spRenderer.sortingOrder = 1;
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

    public void StartFalling(Vector3 position)
    {
        StartCoroutine(Falling(position));
    }

    public void Damaged()
    {
        health--;
        StartCoroutine(ChangeEnemyColor());
        StartCoroutine(ChangeTimeScale());
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
        inGameShadow = (GameObject) Instantiate(muscleliniShadow, transform.position, transform.rotation);
        spRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    IEnumerator ChangeEnemyColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.3f, 0, 0);
        yield return new WaitForSecondsRealtime(0.05f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }

    IEnumerator ChangeTimeScale()
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(0.03f); ;
        Time.timeScale = 1;
    }

    public IEnumerator Falling(Vector3 positionToFall)
    {
        transform.position = positionToFall;
        yield return new WaitForSecondsRealtime(1f);
        spRenderer.enabled = true;
        anim.SetBool("Fall", true);
        Destroy(inGameShadow);
        CameraShake(1f, 2f);
        if (playerInsideArea)
        {
            player.GetComponent<PlayerBehaviour>().Damaged(gameObject);
        }
        boxCollider.enabled = true;
        yield return new WaitForSecondsRealtime(1f);
        anim.SetBool("Fall", false);
        currentAttack = MuscleliniAttacks.PunchAttack;
        jumpingCoroutineRunning = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            playerInsideArea = true;
        }
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
