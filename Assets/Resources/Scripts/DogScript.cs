using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogScript : MonoBehaviour
{
    [SerializeField]
    AnimationClip biteClip;
    GameObject player;
    Animator anim;
    bool biting;
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        biting = false;
	}

	void Update ()
    {
        ChangeAnimation();
        this.GetComponent<PolyNavAgent>().SetDestination(player.transform.position);
	}

    void ChangeAnimation()
    {
        Vector3 diferenceVector = player.transform.position - transform.position;
        if(Mathf.Abs(diferenceVector.x) < Mathf.Abs(diferenceVector.y))
        {
            if (player.transform.position.y > transform.position.y)
                anim.SetFloat("WalkingFloat", 0);
            else
                anim.SetFloat("WalkingFloat", 2);
        }
        else
        {
            if (player.transform.position.x > transform.position.x)
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
    void Bite()
    {
        StartCoroutine(IBite());
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
    
}
