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
        [HideInInspector]
        public BoxColliderTriggerScript boxColliderTrigger;
        [HideInInspector]
        public Vector3 startingPoint;
        Vector3 lastFrameVector;
        Animator anim;
        float health;
        float biteTime;
        bool biting;
        bool damaged;
        bool isDead;
    #endregion

    public void Start ()
    {
        boxColliderTrigger = transform.parent.FindChild("BoxColliderTrigger").GetComponent<BoxColliderTriggerScript>();
        health = 3;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        biting = false;
	}

	void Update ()
    {
        if (!isDead)
        {
            if (!damaged)
                ChangeAnimation(anim);
            biteTime += Time.deltaTime;
            GetComponent<PolyNavAgent>().SetDestination(player.transform.position);
        }

        if (!boxColliderTrigger.gameObject.GetComponent<Collider2D>().bounds.Contains(transform.position))
        {
            transform.position = startingPoint;
        }
    }

    public void ChangeAnimation(Animator anim)
    {
        Vector3 diferenceVector = GetComponent<PolyNavAgent>().movingDirection;
        if (Mathf.Abs(diferenceVector.x) < Mathf.Abs(diferenceVector.y))
        {
            if (diferenceVector.y > 0)
                anim.SetFloat("WalkingFloat", 0);
            else
                anim.SetFloat("WalkingFloat", 2);
        }
        else
        {
            if (diferenceVector.x > 0)
                anim.SetFloat("WalkingFloat", 1);
            else
                anim.SetFloat("WalkingFloat", 3);
        }
    }
    
    public void OtherChangeAnimation(Transform toLookAt, Animator anim)
    {
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
        } }

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
            isDead = true;
            Destroy(GetComponent<Collider2D>());
            anim.SetTrigger("Die");
            GetComponent<PolyNavAgent>().Stop();
            Time.timeScale = 1;
            boxColliderTrigger.numberOfEnemiesInRoom--;
            GetComponent<SpriteRenderer>().sortingLayerName = "DeadEnemy";
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
        Vector3 auxVector = new Vector3(0, 0);
        if (tank.transform.position.x < transform.position.x)
        {
            if (tank.transform.position.y < transform.position.y)
            {
                auxVector = new Vector3(-1F, -1F);
            }
            else if (tank.transform.position.y > transform.position.y)
            {
                auxVector = new Vector3(-1F, 1F);
            }
        }
        else if (tank.transform.position.x > transform.position.x)
        {
            if (tank.transform.position.y < transform.position.y)
            {
                auxVector = new Vector3(1F, -1F);
            }
            else if (tank.transform.position.y > transform.position.y)
            {
                auxVector = new Vector3(1F, 1F);
            }
        }
        Vector3 diffVector = tank.GetComponent<TankScript>().differenceVector.normalized;
        Vector3 toSum = diffVector + auxVector;
        for (int i = 0; i < 10; i++)
        {
            transform.position += toSum * 0.25f;
            yield return new WaitForSeconds(0.001f);
        }
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
        if (col.tag.Equals("Player") && col.GetComponent<PlayerBehaviour>().canBeHitten && !col.GetComponent<PlayerBehaviour>().dead && !isDead && biteTime > 1f)
        {
            if (!biting) Bite();
            biteTime = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Bullet") && GetComponent<DogScript>().isActiveAndEnabled && !isDead)
        {
            Damaged();
            Destroy(col.gameObject);
        }
    }

}
