using UnityEngine;
using System.Collections;
using System;

public class PlayerBehaviour : MonoBehaviour 
{
    #region Variables
        [SerializeField]
        float speed;
        [SerializeField]
        GameObject aux;
        [SerializeField]
        GameObject body;
        [SerializeField]
        GameObject legs;
        [SerializeField]
        GameObject gun;
        [SerializeField]
        GameObject bullet;
        [SerializeField]
        AnimationClip animShottingClip;
        [SerializeField]
        Transform mainCamera;
	    [SerializeField]
	    GameObject gameManager;
        [SerializeField]
        AudioSource walkingAudio;
        [SerializeField]
        AudioSource gunAudio;
        [SerializeField]
        AnimationClip rollingAnimation;
  
        Animator bodyAnim;
        Animator legsAnim;
        Animator gunAnim;
        SpriteRenderer bodySpriteRenderer;
        SpriteRenderer legsSpriteRenderer;
        SpriteRenderer gunSpriteRenderer;
        GameObject door;
    
        public GameObject currentRoom;
        public bool playerCanMove;
        public bool canBeHitten;
        public bool dead;
        private bool collidingWithDoor;
	    private float shakeDuration;
	    private float shakeAmount;
	    private float decreaseFactor;
	    private Vector3 camPosition;
	    private bool canShock;
        private bool rolling;
        float angle;
        float time = 1;
        float rollTime;
        float w;
        float h;
        public int health = 15;
        float walkingDirX;
        float walkingDirY;
        bool canEnterDoor = true;
    #endregion

    void Awake()
    {
        playerCanMove = true;
        canBeHitten = true;
        dead = false;
        Cursor.visible = true;
        bodyAnim = body.GetComponent<Animator>();
        legsAnim = legs.GetComponent<Animator>();
        gunAnim = gun.GetComponent<Animator>();
        bodySpriteRenderer = body.GetComponent<SpriteRenderer>();
        legsSpriteRenderer = legs.GetComponent<SpriteRenderer>();
        gunSpriteRenderer = gun.GetComponent<SpriteRenderer>();
        mainCamera = Camera.main.transform;
        shakeDuration = 0;
        shakeAmount = 0.3f;
        decreaseFactor = 1;
    }

	void Update() 
	{
        if (playerCanMove && Time.deltaTime != 0 && !dead)
        {
            time += Time.deltaTime;
            rollTime += Time.deltaTime;
            w = Input.GetAxis("Horizontal");
            h = Input.GetAxis("Vertical");
            ChangeMovement();
            ChangeRotation();
            CheckDifferentVisions();
            CameraShake();
            if(Input.GetMouseButton(1) && rollTime >= 0.5f)
            {
                StartCoroutine(Rolling(w, h));
                rollTime = 0;
            }
            if (Input.GetMouseButton(0) && time >= 0.8 && canBeHitten && !rolling)
            {
                time = 0;
                gunAnim.SetBool("Shotting", true);
                bodyAnim.SetBool("Shotting", true);
                shakeDuration = 0.1f;
                ShotBullet();
            }
            else if (time >= 0.005)
            {
                gunAnim.SetBool("Shotting", false);
                bodyAnim.SetBool("Shotting", false);
            }
        }
        if(health <= 0 && !dead)
        {
            gun.SetActive(false);
            legs.SetActive(false);
            bodyAnim.SetTrigger("Die");
            Destroy(GetComponent<Collider2D>());
            dead = true;
        }
	}

    void ChangeMovement()
    {
        if (canBeHitten && !rolling)
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                bodyAnim.SetBool("Walking", true);
                legsAnim.SetBool("Walking", true);
                this.transform.position += (Vector3.up + Vector3.right) * speed * Time.deltaTime;
                legsAnim.SetFloat("WalkingLeg", 2);
                walkingAudio.UnPause();
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                bodyAnim.SetBool("Walking", true);
                legsAnim.SetBool("Walking", true);
                this.transform.position += (Vector3.up + Vector3.left) * speed * Time.deltaTime;
                legsAnim.SetFloat("WalkingLeg", 2);
                walkingAudio.UnPause();
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                bodyAnim.SetBool("Walking", true);
                legsAnim.SetBool("Walking", true);
                this.transform.position += (Vector3.down + Vector3.right) * speed * Time.deltaTime;
                legsAnim.SetFloat("WalkingLeg", 0);
                walkingAudio.UnPause();
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                bodyAnim.SetBool("Walking", true);
                legsAnim.SetBool("Walking", true);
                this.transform.position += (Vector3.down + Vector3.left) * speed * Time.deltaTime;
                legsAnim.SetFloat("WalkingLeg", 0);
                walkingAudio.UnPause();
            }
            else if (Input.GetKey(KeyCode.W))
            {
                bodyAnim.SetBool("Walking", true);
                legsAnim.SetBool("Walking", true);
                this.transform.position += Vector3.up * speed * Time.deltaTime;
                legsAnim.SetFloat("WalkingLeg", 2);
                walkingAudio.UnPause();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                bodyAnim.SetBool("Walking", true);
                legsAnim.SetBool("Walking", true);
                this.transform.position += Vector3.right * speed * Time.deltaTime;
                legsAnim.SetFloat("WalkingLeg", 1);
                walkingAudio.UnPause();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                bodyAnim.SetBool("Walking", true);
                legsAnim.SetBool("Walking", true);
                this.transform.position += Vector3.left * speed * Time.deltaTime;
                legsAnim.SetFloat("WalkingLeg", 3);
                walkingAudio.UnPause();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                bodyAnim.SetBool("Walking", true);
                legsAnim.SetBool("Walking", true);
                this.transform.position += Vector3.down * speed * Time.deltaTime;
                legsAnim.SetFloat("WalkingLeg", 0);
                walkingAudio.UnPause();
            }
            else
            {
                bodyAnim.SetBool("Walking", false);
                legsAnim.SetBool("Walking", false);
                walkingAudio.Pause();
            }
        }
    }

    void ChangeRotation()
    {
        angle = aux.transform.eulerAngles.z;
        if (angle > 157.5 && angle < 202.5)
        {
            bodyAnim.SetFloat("LookingAngle", 0);
            gunAnim.SetFloat("GunPosition", 0);

            GetComponent<BoxCollider2D>().offset = new Vector2(0.001316424f, -0.07305329f);
            GetComponent<BoxCollider2D>().size = new Vector2(0.4753902f, 0.6980875f);

        }
        else if (angle > 247.5 && angle < 292.5)
        {
            bodyAnim.SetFloat("LookingAngle", 1);
            gunAnim.SetFloat("GunPosition", 1);

            GetComponent<BoxCollider2D>().offset = new Vector2(0.13f, -0.07305329f);
            GetComponent<BoxCollider2D>().size = new Vector2(0.64f, 0.6980875f);
        }
        else if (angle > 67.5 && angle < 112.5)
        {
            bodyAnim.SetFloat("LookingAngle", 3);
            gunAnim.SetFloat("GunPosition", 3);

            GetComponent<BoxCollider2D>().offset = new Vector2(-0.12f, -0.07305329f);
            GetComponent<BoxCollider2D>().size = new Vector2(0.64f, 0.6980875f);
        }
        else if(angle >= 22.5 && angle <= 67.5)
        {
            bodyAnim.SetFloat("LookingAngle", 5);
            gunAnim.SetFloat("GunPosition", 5);

            GetComponent<BoxCollider2D>().offset = new Vector2(-0.12f, -0.07305329f);
            GetComponent<BoxCollider2D>().size = new Vector2(0.64f, 0.6980875f);
        }
        else if(angle >= 292.5 && angle <= 337.5)
        {
            bodyAnim.SetFloat("LookingAngle", 4);
            gunAnim.SetFloat("GunPosition", 4);

            GetComponent<BoxCollider2D>().offset = new Vector2(0.13f, -0.07305329f);
            GetComponent<BoxCollider2D>().size = new Vector2(0.64f, 0.6980875f);
        }
        else if(angle >= 112.5 && angle <= 157.5)
        {
            bodyAnim.SetFloat("LookingAngle", 7);
            gunAnim.SetFloat("GunPosition", 7);

            GetComponent<BoxCollider2D>().offset = new Vector2(-0.12f, -0.07305329f);
            GetComponent<BoxCollider2D>().size = new Vector2(0.64f, 0.6980875f);
        }
        else if(angle >= 202.5 && angle <= 247.5)
        {
            bodyAnim.SetFloat("LookingAngle", 6);
            gunAnim.SetFloat("GunPosition", 6);

            GetComponent<BoxCollider2D>().offset = new Vector2(0.13f, -0.07305329f);
            GetComponent<BoxCollider2D>().size = new Vector2(0.64f, 0.6980875f);
        }
        else if (angle < 22.5 || angle > 337.5)
        {
            bodyAnim.SetFloat("LookingAngle", 2);
            gunAnim.SetFloat("GunPosition", 2);

            GetComponent<BoxCollider2D>().offset = new Vector2(0.001316424f, -0.07305329f);
            GetComponent<BoxCollider2D>().size = new Vector2(0.4753902f, 0.6980875f);
        }
    }

    void CheckDifferentVisions()
    {
        if((Input.GetKey(KeyCode.W) && angle > 157.5 && angle < 202.5) || (Input.GetKey(KeyCode.A) && angle > 247.5 && angle < 292.5) || (Input.GetKey(KeyCode.S) && (angle < 22.5 || angle > 337.5)) || (Input.GetKey(KeyCode.D) && angle > 67.5 && angle < 112.5) || (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) && angle >= 292.5 && angle <= 337.5) || (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) && angle >= 22.5 && angle <= 67.5) || (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && angle >= 202.5 && angle <= 247.5) || (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && angle >= 112.5 && angle <= 157.5))
        {
            legsAnim.SetBool("Backwards", true);
        }
        else
        {
            legsAnim.SetBool("Backwards", false);
        }
    }
    
    void CameraShake()
    {
        camPosition = mainCamera.position;
        if (shakeDuration > 0)
        {
            mainCamera.localPosition = camPosition + UnityEngine.Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            mainCamera.localPosition = camPosition;
        }
    }

    public void ShotBullet()
    {
        Instantiate(bullet, gun.transform.position, Quaternion.Euler(0 ,0f, aux.transform.eulerAngles.z + 90f));
        gunAudio.Play();
    }

    public void Damaged(GameObject col)
    {
        if (!dead)
        {
            canBeHitten = false;
            playerCanMove = false;
            bodyAnim.SetBool("Damaged", true);
            legsAnim.SetBool("Damaged", true);
            health--;
            gunSpriteRenderer.color = new Color(gunSpriteRenderer.color.r, gunSpriteRenderer.color.g, gunSpriteRenderer.color.b, 0);
            StartCoroutine(ChangePlayerColor());
            StartCoroutine(GoingBackWhenShot(col));
        }
    }

    IEnumerator ChangePlayerColor()
    {
        gunSpriteRenderer.enabled = false;
        for (int i = 0; i < 10; i++)
        {
            if (bodySpriteRenderer.color.a.Equals(1))
            {
                bodySpriteRenderer.color = new Color(bodySpriteRenderer.color.r, bodySpriteRenderer.color.g, bodySpriteRenderer.color.b, 0);
                legsSpriteRenderer.color = new Color(legsSpriteRenderer.color.r, legsSpriteRenderer.color.g, legsSpriteRenderer.color.b, 0);
            }
            else
            {
                bodySpriteRenderer.color = new Color(bodySpriteRenderer.color.r, bodySpriteRenderer.color.g, bodySpriteRenderer.color.b, 1);
                legsSpriteRenderer.color = new Color(legsSpriteRenderer.color.r, legsSpriteRenderer.color.g, legsSpriteRenderer.color.b, 1);
            }
            yield return new WaitForSecondsRealtime(0.05f);
        }
        gunSpriteRenderer.enabled = true;
        gunSpriteRenderer.color = new Color(gunSpriteRenderer.color.r, gunSpriteRenderer.color.g, gunSpriteRenderer.color.b, 1);
        canBeHitten = true;
        playerCanMove = true;
    }

    IEnumerator GoingBackWhenShot(GameObject aux)
    {
        float multiplier;
        if (!aux.tag.Equals("Boss") || !aux.tag.Equals("Tank"))
        {
            multiplier = 0.2f;
        }
        else
        {
            multiplier = 0.7f;
        }
        Vector3 goingDirection = aux.transform.position - transform.position;
        Vector3 goBackVector = new Vector3(-goingDirection.normalized.x, -goingDirection.normalized.y) * multiplier;
        for (float i = 0; i < 10; i++)
        {
            transform.position += goBackVector;
            yield return new WaitForSecondsRealtime(0.001f);
        }
        if (goBackVector.x > 0f)
        {
            bodyAnim.SetFloat("DamagedFloat", 0);
            legsAnim.SetFloat("DamagedFloat", 0);
        }
        else if (goBackVector.x < 0f)
        {
            bodyAnim.SetFloat("DamagedFloat", 1);
            legsAnim.SetFloat("DamagedFloat", 1);
        }
        yield return new WaitForSeconds(0.25f);
        bodyAnim.SetBool("Damaged", false);
        legsAnim.SetBool("Damaged", false);
    }

    IEnumerator Rolling(float dirX, float dirY)
    {
        rolling = true;
        Vector3 toRollVector = new Vector3(dirX, dirY);
        bodyAnim.SetTrigger("Roll");
        gunSpriteRenderer.enabled = false;
        legsSpriteRenderer.enabled = false;
        for(int i = 0; i <= 10; i++)
        {
            mainCamera.transform.position = new Vector3(transform.position.x,transform.position.y, mainCamera.transform.position.z);
            transform.position += toRollVector * speed * 0.1f;
            yield return new WaitForSecondsRealtime(0.000001f);
        }
        yield return new WaitForSecondsRealtime(0.2f);
        gunSpriteRenderer.enabled = true;
        legsSpriteRenderer.enabled = true;
        rolling = false;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag.Equals("Door") && canEnterDoor)
        {
            door = col.gameObject;
            door.GetComponent<DoorScript>().ChangeRoom();
            canEnterDoor = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag.Equals("Door"))
        {
            if(!col.gameObject.Equals(door))
            {
                canEnterDoor = true;
            }
            door = null;
        }
    }
}
