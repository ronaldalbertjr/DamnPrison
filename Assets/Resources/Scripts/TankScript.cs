using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour
{
    [SerializeField]
    AnimationClip hitAnimClip;
    [SerializeField]
    float speed;
    GameObject spawner;
    GameObject player;
    Animator anim;
    Vector3 runDirection;
	SpriteRenderer spRenderer;
    bool running;
	bool canCollide;
	bool facingRight;
	float shakeDuration;
	int health;
	void Start ()
    {
        running = true;
		health = 10;
		spRenderer = GetComponent<SpriteRenderer> ();
        player = GameObject.FindGameObjectWithTag("Player");
        spawner = GameObject.Find("spawnPoints");
        anim = GetComponent<Animator>();
        runDirection = player.transform.position - transform.position;
		canCollide = true;
		anim.SetBool ("Running", true);
	}
	void Update ()
    {
		if (running) 
		{
			transform.position += runDirection.normalized * Time.deltaTime * speed;
		} 
		else 
		{
			CameraShake (0.3f, 1f);
			Flip ();
		}
		if (health <= 0) 
		{
            spawner.GetComponent<SpawnEnemy>().Instantiate();
			Time.timeScale = 1f;
			Destroy (gameObject);
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (canCollide) 
		{
			shakeDuration = 0.5f;
			StartCoroutine (Collision ());
			canCollide = false;
		}
    }

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag.Equals ("Bullet")) 
		{
			health--;
			Destroy (col.gameObject);
			StartCoroutine (ChangeEnemyColor ());
			StartCoroutine (ChangeTimeScale ());
		}
	}
	void Flip()
	{
		if (player.transform.position.x > transform.position.x && !facingRight) 
		{
			facingRight = true;
			ChangeScale (facingRight);
		}
		else if (player.transform.position.x < transform.position.x && facingRight) 
		{
			facingRight = false;
			ChangeScale (facingRight);
		}
	}

	void ChangeScale(bool flip)
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
        anim.SetBool("Running", false);
        running = false;
        yield return new WaitForSeconds(hitAnimClip.length * 2);
		anim.SetBool("Running", true);
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
			yield return new WaitForSeconds(0.001f);
		}
	}


	IEnumerator ChangeEnemyColor()
	{
		GetComponent<SpriteRenderer>().color = new Color(0.3f, 0, 0);
		yield return new WaitForSeconds(0.05f);
		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
		yield return 0;
	}

	IEnumerator ChangeTimeScale()
	{
		Time.timeScale = 0.3f;
		yield return new WaitForSeconds(0.03f); ;
		Time.timeScale = 1;
	}


}
