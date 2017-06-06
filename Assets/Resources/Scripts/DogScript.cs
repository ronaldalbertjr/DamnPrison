using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogScript : MonoBehaviour
{
    [SerializeField]
    AnimationClip biteClip;
    GameObject spawner;
    GameObject player;
    BoxColliderTriggerScript boxColliderTrigger;
    Animator anim;
    float health;
    bool biting;
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
        ChangeAnimation(player.transform, anim);
        this.GetComponent<PolyNavAgent>().SetDestination(player.transform.position);
	}

    public void ChangeAnimation(Transform toLookAt, Animator anim)
    {
        Vector3 diferenceVector = toLookAt.position - transform.position;
        if(Mathf.Abs(diferenceVector.x) < Mathf.Abs(diferenceVector.y))
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

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag.Equals("Player"))
        {
            if(!biting) Bite();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag.Equals("Bullet") &&  GetComponent<DogScript>().isActiveAndEnabled)
        {
            Damaged();
            Destroy(col.gameObject);
        }
    }
    void Bite()
    {
        StartCoroutine(IBite());
    }
    
    public void Damaged()
    {
        health--;
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

}
