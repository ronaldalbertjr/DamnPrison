using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogScript : MonoBehaviour
{
    #region Variables
        [SerializeField]
        AnimationClip biteClip;
        GameObject spawner;
        GameObject player;
        BoxColliderTriggerScript boxColliderTrigger;
        Vector3 lastFrameVector;
        Animator anim;
        float health;
        bool biting;
        bool damaged;
    #endregion

    public void Start ()
    {
        boxColliderTrigger = transform.parent.FindChild("BoxColliderTrigger").GetComponent<BoxColliderTriggerScript>();
        health = 5;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        biting = false;
	}

	void Update ()
    {
        if(!damaged)
            ChangeAnimation(player.transform, anim);
        GetComponent<PolyNavAgent>().SetDestination(player.transform.position);
	}

    public void ChangeAnimation(Transform toLookAt, Animator anim)
    {
        /*
        Vector3 diferenceVector = transform.position - lastFrameVector;
        if(Mathf.Abs(diferenceVector.x) < Mathf.Abs(diferenceVector.y))
        {
            if (lastFrameVector.y < transform.position.y)
                anim.SetFloat("WalkingFloat", 0);
            else
                anim.SetFloat("WalkingFloat", 2);
        }
        else
        {
            if (lastFrameVector.x < transform.position.x)
                anim.SetFloat("WalkingFloat", 1);
            else
                anim.SetFloat("WalkingFloat", 3);
        }
        lastFrameVector = transform.position;
        */
        Vector3 diferenceVector = toLookAt.position - transform.position;
        if (Mathf.Abs(diferenceVector.x) < Mathf.Abs(diferenceVector.y))
        {
            if (toLookAt.position.y > transform.position.y)
                anim.SetFloat("WalkingFloat", 0);
            else
                anim.SetFloat("WalkingFloat", 2);
        }
        else
        {
            if (toLookAt.position.x > transform.position.x)
                anim.SetFloat("WalkingFloat", 1);
            else
                anim.SetFloat("WalkingFloat", 3);
        }
    }

    void Bite()
    {
        StartCoroutine(IBite());
    }
    
    public void Damaged(GameObject tank = null, bool hittenByTank = false)
    {
        damaged = true;
        health--;
        if (hittenByTank)
            StartCoroutine(HittenByTank(tank));
        else
            StartCoroutine(GoingBackWhenShot());
        StartCoroutine(ChangeEnemyColor());
        StartCoroutine(ChangeTimeScale());
        if (health <= 0)
        {
            Destroy(gameObject);
            Time.timeScale = 1;
            boxColliderTrigger.numberOfEnemiesInRoom--;
        }
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

    IEnumerator IBite()
    {
        biting = true;
        anim.SetTrigger("Bite");
        yield return new WaitForSecondsRealtime(biteClip.length/2);
        player.GetComponent<PlayerBehaviour>().Damaged(gameObject);
        yield return new WaitForSecondsRealtime(biteClip.length/2);
        biting = false;
    }

    IEnumerator GoingBackWhenShot()
    {
        float multiplier = 0.08f;
        Vector3 goingDirection = player.transform.position - transform.position;
        Vector3 goBackVector = new Vector3(-goingDirection.normalized.x, -goingDirection.normalized.y) * multiplier;
        for (float i = 0; i < 10; i++)
        {
            transform.position += goBackVector;
            yield return new WaitForSecondsRealtime(0.001f);
        }
        yield return new WaitForSeconds(0.5f);
        damaged = false;
    }

    IEnumerator HittenByTank(GameObject tank)
    {
        Vector3 differenceVector = transform.position - tank.transform.position;
        if (Mathf.Abs(differenceVector.x) < Mathf.Abs(differenceVector.y))
        {
            if (transform.position.x < tank.transform.position.x)
            {
                for (int i = 0; i < 10; i++)
                {
                    transform.position += new Vector3(0, -1) * 0.5f;
                    yield return new WaitForSeconds(0.001f);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    transform.position += new Vector3(0, 1) * 0.5f;
                    yield return new WaitForSeconds(0.001f);
                }
            }
        }

        else
        {
            if (transform.position.y < tank.transform.position.y)
            {
                for (int i = 0; i < 10; i++)
                {
                    transform.position += new Vector3(-1, 0) * 0.5f;
                    yield return new WaitForSeconds(0.001f);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    transform.position += new Vector3(1, 0) * 0.5f;
                    yield return new WaitForSeconds(0.001f);
                }
            }
        }
        anim.SetBool("Shot", false);
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
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            if (!biting) Bite();
        }

        if (col.tag.Equals("Untouchable"))
        {
            StartCoroutine(ChangeWalkingDirection(col.gameObject));
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Bullet") && GetComponent<DogScript>().isActiveAndEnabled)
        {
            Damaged();
            Destroy(col.gameObject);
        }
    }

}
