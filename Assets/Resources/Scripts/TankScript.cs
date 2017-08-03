using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour
{
    #region Variables
        [SerializeField]
        AnimationClip hitAnimClip;
        [SerializeField]
        AnimationClip standingUpClip;
        [SerializeField]
        AnimationClip dyingClip;
        [SerializeField]
        AudioSource tankCollidedWalkAudio;
        [SerializeField]
        float speed;
        GameObject spawner;
        GameObject player;
        Animator anim;
        Vector3 runDirection;
        [HideInInspector]
        public Vector3 differenceVector;
        Vector3 lastFrameVector;
	    SpriteRenderer spRenderer;
        [HideInInspector]
        public BoxColliderTriggerScript boxColliderTrigger;
        bool running;
        bool canRun;
	    bool canCollide;
	    bool facingRight;
        bool invencible;
        bool die;
	    float shakeDuration;
	    int health;
    #endregion

    public void Start()
    {
        boxColliderTrigger = transform.parent.FindChild("BoxColliderTrigger").GetComponent<BoxColliderTriggerScript>();
        running = true;
		health = 10;
		spRenderer = GetComponent<SpriteRenderer> ();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        runDirection = player.transform.position - transform.position;
		canCollide = true;
        canRun = true;
        die = false;
        anim.SetBool("Walking", false);
		anim.SetBool ("Running", true);
        Flip(player.transform, spRenderer);
        lastFrameVector = transform.position;
        GetComponent<PolyNavAgent>().OnDestinationReached += setCanRunTrue;
    }

	void Update()
    {
        differenceVector = transform.position - lastFrameVector;
        lastFrameVector = transform.position;
        if (running && !die && anim.GetBool("Running"))
        {
            if (canRun)
            {
                GetComponent<PolyNavAgent>().Stop();
                transform.position += runDirection.normalized * Time.deltaTime * speed;
                anim.SetBool("Walking", false);
                anim.SetBool("Running", true);
            }
            else if (!canRun && !GetComponent<PolyNavAgent>().hasPath)
            {
                ChangePositionUntilItCanRun();
                anim.SetBool("Running", false);
                anim.SetBool("Walking", true);
            }
        }
        else if(!die)
        {
            CameraShake(0.3f, 1f);
            Flip(player.transform, spRenderer);
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
    
    void CheckRun()
    {
        Vector3 checkDirection = player.transform.position - transform.position;
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, checkDirection, Mathf.Infinity, layerMask);
        if(hit.collider.tag.Equals("Player"))
            canRun = true;
        else
            canRun = false;
    }
    
    void setCanRunTrue()
    {
        canRun = true;
    }
    
    void ChangePositionUntilItCanRun()
    {
        Queue<Vector3> queue = new Queue<Vector3>();
        HashSet<Vector3> path = new HashSet<Vector3>();
        Vector3 positionToWalkTo = new Vector3(0,0,0);
        Vector3 checkDirection = new Vector3(0, 0, 0);
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        queue.Enqueue(transform.position);
        while (queue.Count > 0)
        {
            Vector3 positionDequeued = queue.Dequeue();
            checkDirection = player.transform.position - positionDequeued;
            RaycastHit2D hit = Physics2D.Raycast(positionDequeued, checkDirection, Mathf.Infinity, layerMask);
            if (hit.collider.tag.Equals("Player"))
            {
                positionToWalkTo = positionDequeued;
                GetComponent<PolyNavAgent>().SetDestination(positionToWalkTo);
                Debug.DrawLine(transform.position, positionToWalkTo, Color.red, 20);
                Debug.DrawLine(positionDequeued, hit.collider.transform.position, Color.red, 20);
                break;
            }
            else
            {
                if (boxColliderTrigger.GetComponent<Collider2D>().bounds.Contains(positionDequeued + new Vector3(0.1f, 0)) && !path.Contains(positionDequeued + new Vector3(0.1f, 0)))
                {
                    queue.Enqueue(positionDequeued + new Vector3(0.1f, 0));
                    path.Add(positionDequeued + new Vector3(0.1f, 0));
                }
                if (boxColliderTrigger.GetComponent<Collider2D>().bounds.Contains(positionDequeued + new Vector3(0, 0.1f)) && !path.Contains(positionDequeued + new Vector3(0, 0.1f)))
                {
                    queue.Enqueue(positionDequeued + new Vector3(0, 0.1f));
                    path.Add(positionDequeued + new Vector3(0, 0.1f));
                }
                if (boxColliderTrigger.GetComponent<Collider2D>().bounds.Contains(positionDequeued + new Vector3(-0.1f, 0)) && !path.Contains(positionDequeued + new Vector3(-0.1f, 0)))
                {
                    queue.Enqueue(positionDequeued + new Vector3(-0.1f, 0));
                    path.Add(positionDequeued + new Vector3(-0.1f, 0));
                }
                if (boxColliderTrigger.GetComponent<Collider2D>().bounds.Contains(positionDequeued + new Vector3(0, -0.1f)) && !path.Contains(positionDequeued + new Vector3(0, -0.1f)))
                {
                    queue.Enqueue(positionDequeued + new Vector3(0, -0.1f));
                    path.Add(positionDequeued + new Vector3(0, -0.1f));
                }
            }
        }
        if(!boxColliderTrigger.GetComponent<Collider2D>().bounds.Contains(positionToWalkTo + new Vector3(1f, 0)))
        {
            positionToWalkTo += new Vector3(-1f, 0);
        }
        if (!boxColliderTrigger.GetComponent<Collider2D>().bounds.Contains(positionToWalkTo + new Vector3(-1f, 0)))
        {
            positionToWalkTo += new Vector3(1f, 0);
        }
        if (!boxColliderTrigger.GetComponent<Collider2D>().bounds.Contains(positionToWalkTo + new Vector3(0, 1f)))
        {
            positionToWalkTo += new Vector3(0, -1f);
        }
        if (!boxColliderTrigger.GetComponent<Collider2D>().bounds.Contains(positionToWalkTo + new Vector3(0, -1f)))
        {
            positionToWalkTo += new Vector3(0, 1f);
        }

        GetComponent<PolyNavAgent>().SetDestination(positionToWalkTo);
    }

    IEnumerator Collision()
    {
        canRun = false;
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
        CheckRun();
        Destroy(GetComponent<PolyNavObstacle>());
        running = true;
		canCollide = true;
    }

	IEnumerator GoingBackWhenCollided()
	{
		Vector3 goBackVector = new Vector3(-runDirection.normalized.x, -runDirection.normalized.y) * 0.3f;
		for (float i = 0; i < 10; i++)
		{
			transform.position += goBackVector;
			yield return new WaitForSecondsRealtime(0.001f);
		}
        gameObject.AddComponent<PolyNavObstacle>();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(GetComponent<PolyNavAgent>().hasPath)
        {
            canRun = true;
            anim.SetBool("Running", true);
        }
        if (canCollide && (collision.gameObject.tag.Equals("Background") || collision.gameObject.tag.Equals("Door")) && !GetComponent<PolyNavAgent>().hasPath)
        {
            shakeDuration = 0.5f;
            StartCoroutine(Collision());
            tankCollidedWalkAudio.Play();
            canCollide = false;
        }
        else if (canCollide && collision.gameObject.tag.Equals("Player") && collision.gameObject.GetComponent<PlayerBehaviour>().playerCanMove && !GetComponent<PolyNavAgent>().hasPath && !die)
        {
            player.GetComponent<PlayerBehaviour>().Damaged(gameObject);
        }
        else if (collision.gameObject.tag.Equals("Porrete") && running && !GetComponent<PolyNavAgent>().hasPath && !die)
        {
            collision.gameObject.GetComponent<PorreteScript>().Damaged(this.gameObject, true);
        }
        else if (collision.gameObject.tag.Equals("EnemyGun") && running && !GetComponent<PolyNavAgent>().hasPath && !die)
        {
            collision.gameObject.GetComponent<RangedEnemyScript>().Damaged(this.gameObject, true);
        }
        else if (collision.gameObject.tag.Equals("Dog") && running && !GetComponent<PolyNavAgent>().hasPath)
        {
            collision.gameObject.GetComponent<DogScript>().Damaged(this.gameObject, true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (canCollide && (collision.gameObject.tag.Equals("Background") || collision.gameObject.tag.Equals("Door")) && !GetComponent<PolyNavAgent>().hasPath)
        {
            shakeDuration = 0.5f;
            StartCoroutine(Collision());
            tankCollidedWalkAudio.Play();
            canCollide = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Bullet") && GetComponent<TankScript>().isActiveAndEnabled && !die)
        {
            Destroy(col.gameObject);
            if (health <= 0)
            {
                gameObject.AddComponent<PolyNavObstacle>();
                Destroy(gameObject.GetComponent<Collider2D>());
                canCollide = false;
                running = false;
                die = true;
                Time.timeScale = 1f;
                boxColliderTrigger.numberOfEnemiesInRoom--;
                anim.SetTrigger("Die");
            }
            else
                Damaged();
        }
    }
}
