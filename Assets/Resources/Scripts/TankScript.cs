using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour
{
    [SerializeField]
    AnimationClip hitAnimClip;
    [SerializeField]
    AnimationClip standingUpClip;
    [SerializeField]
    AnimationClip dyingClip;
    [SerializeField]
    float speed;
    GameObject spawner;
    GameObject player;
    Animator anim;
    Vector3 runDirection;
	SpriteRenderer spRenderer;
    BoxColliderTriggerScript boxColliderTrigger;
    bool running;
	bool canCollide;
	bool facingRight;
    bool invencible;
    bool dying;
	float shakeDuration;
	int health;
	public void Start ()
    {
        boxColliderTrigger = transform.parent.FindChild("BoxColliderTrigger").GetComponent<BoxColliderTriggerScript>();
        running = true;
		health = 20;
		spRenderer = GetComponent<SpriteRenderer> ();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        runDirection = player.transform.position - transform.position;
		canCollide = true;
		anim.SetBool ("Running", true);
        Flip(player.transform, spRenderer);
	}
	void Update ()
    {
		if (running && !dying) 
		{
			transform.position += runDirection.normalized * Time.deltaTime * speed;
		} 
		else
		{
			CameraShake (0.3f, 1f);
			Flip (player.transform, spRenderer);
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canCollide && (collision.gameObject.tag.Equals("Background") || collision.gameObject.tag.Equals("Door")))  
		{
			shakeDuration = 0.5f;
			StartCoroutine (Collision());
			canCollide = false;
		}
        else if(canCollide && collision.gameObject.tag.Equals("Player"))
        {
            player.GetComponent<PlayerBehaviour>().Damaged(collision.gameObject);
        }
    }

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag.Equals ("Bullet") && GetComponent<TankScript>().isActiveAndEnabled && !dying) 
		{
            Damaged();
			Destroy (col.gameObject);
            if (health <= 0)
            {
                dying = true;
                Time.timeScale = 1f;
                boxColliderTrigger.numberOfEnemiesInRoom--;
                anim.SetTrigger("Die");
                Invoke("DestroyTank", dyingClip.length);
            }
        }
	}
    private void DestroyTank()
    {
		Destroy(gameObject);
    }
    public void Damaged()
    {
        health--;
        StartCoroutine(ChangeEnemyColor());
        StartCoroutine(ChangeTimeScale());
    }
	public void Flip(Transform toLookAt, SpriteRenderer spRenderer)
	{
		if (toLookAt.position.x > transform.position.x && !facingRight) 
		{
			facingRight = true;
			ChangeScale (facingRight, spRenderer);
		}
		else if (toLookAt.position.x < transform.position.x && facingRight) 
		{
			facingRight = false;
			ChangeScale (facingRight, spRenderer);
		}
	}

	void ChangeScale(bool flip, SpriteRenderer spRenderer)
	{
		spRenderer.flipX = flip;
	}

	void CameraShake(float shakeAmount, float decreaseFactor)
	{
		Vector3 camPosition = Camera.main.transform.position;
		if (shakeDuration > 0)
		{
			Camera.main.transform.localPosition = camPosition + Random.insideUnitSphere * shakeAmount;
			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			Camera.main.transform.localPosition = camPosition;
		}
	}
    IEnumerator Collision()
    {
		StartCoroutine (GoingBackWhenCollided ());
        anim.SetTrigger("Hit");
        anim.SetBool("HitWall", true);
        anim.SetBool("Running", false);
        running = false;
        yield return new WaitForSecondsRealtime(hitAnimClip.length * 2);
        anim.SetBool("Running", true);
        yield return new WaitForSecondsRealtime(2.5f);
        anim.SetBool("HitWall", false);
        yield return new WaitForSecondsRealtime(standingUpClip.length * 2f);
		runDirection = player.transform.position - transform.position;
        running = true;
		canCollide = true;
    }

	IEnumerator GoingBackWhenCollided()
	{
		Vector3 goBackVector = new Vector3(-runDirection.normalized.x, -runDirection.normalized.y) * 0.25f;
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
