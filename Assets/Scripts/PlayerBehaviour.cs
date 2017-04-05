using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour 
{
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

    Animator bodyAnim;
    Animator legsAnim;
    Animator gunAnim;

    [HideInInspector]
    public GameObject currentRoom;
	private float shakeDuration;
	private float shakeAmount;
	private float decreaseFactor;
	private Vector3 camPosition;
	private bool canShock;
    float angle;
    float time = 1;

    void Awake()
    {
        Cursor.visible = true;
        bodyAnim = body.GetComponent<Animator>();
        legsAnim = legs.GetComponent<Animator>();
        gunAnim = gun.GetComponent<Animator>();
        mainCamera = Camera.main.transform;
        shakeDuration = 0;
        shakeAmount = 0.7f;
        decreaseFactor = 1;
    }
	void Update () 
	{
        time += Time.deltaTime;
        ChangeMovement();
        ChangeRotation();
        CameraShake();
        if(Input.GetMouseButton(0) && time >= 0.8)
        {
            time = 0;
            gunAnim.SetBool("Shotting", true);
            bodyAnim.SetBool("Shotting", true);
            shakeDuration = 0.1f;
            ShotBullet();
        }
        else if(time >= 0.005)
        {
            gunAnim.SetBool("Shotting", false);
            bodyAnim.SetBool("Shotting", false);
        }
	}

    void ChangeMovement()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            this.transform.position += (Vector3.up + Vector3.right) * speed * Time.deltaTime;
            legsAnim.SetFloat("WalkingLeg", 2);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            this.transform.position += (Vector3.up + Vector3.left) * speed * Time.deltaTime;
            legsAnim.SetFloat("WalkingLeg", 2);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            this.transform.position += (Vector3.down + Vector3.right) * speed * Time.deltaTime;
            legsAnim.SetFloat("WalkingLeg", 0);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            this.transform.position += (Vector3.down + Vector3.left) * speed * Time.deltaTime;
            legsAnim.SetFloat("WalkingLeg", 0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += Vector3.up * speed * Time.deltaTime;
            legsAnim.SetFloat("WalkingLeg", 2);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * speed * Time.deltaTime;
            legsAnim.SetFloat("WalkingLeg", 1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += Vector3.left * speed * Time.deltaTime;
            legsAnim.SetFloat("WalkingLeg", 3);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += Vector3.down * speed * Time.deltaTime;
            legsAnim.SetFloat("WalkingLeg", 0);
        }
    }
    void ChangeRotation()
    {
        angle = aux.transform.eulerAngles.z;
        if (angle > 157.5 && angle < 202.5)
        {
            bodyAnim.SetFloat("WalkingBody", 0);
            gunAnim.SetFloat("GunPosition", 0);
        }
        else if (angle > 247.5 && angle < 292.5)
        {
            bodyAnim.SetFloat("WalkingBody", 1);
            gunAnim.SetFloat("GunPosition", 1);
        }
        else if (angle > 67.5 && angle < 112.5)
        {
            bodyAnim.SetFloat("WalkingBody", 3);
            gunAnim.SetFloat("GunPosition", 3);
        }
        else if(angle >= 22.5 && angle <= 67.5)
        {
            bodyAnim.SetFloat("WalkingBody", 5);
            gunAnim.SetFloat("GunPosition", 5);
        }
        else if(angle >= 292.5 && angle <= 337.5)
        {
            bodyAnim.SetFloat("WalkingBody", 4);
            gunAnim.SetFloat("GunPosition", 4);
        }
        else if(angle >= 112.5 && angle <= 157.5)
        {
            bodyAnim.SetFloat("WalkingBody", 7);
            gunAnim.SetFloat("GunPosition", 7);
        }
        else if(angle >= 202.5 && angle <= 247.5)
        {
            bodyAnim.SetFloat("WalkingBody", 6);
            gunAnim.SetFloat("GunPosition", 6);
        }
        else if (angle < 22.5 || angle > 337.5)
        {
            bodyAnim.SetFloat("WalkingBody", 2);
            gunAnim.SetFloat("GunPosition", 2);
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
            mainCamera.localPosition = camPosition + Random.insideUnitSphere * shakeAmount;
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
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag.Equals("Background"))
        {
            currentRoom = col.transform.gameObject;
        }
    }
}
