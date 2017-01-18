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
    
	Animator bodyAnim;
    Animator legsAnim;
    Animator gunAnim;
    float angle;


    void Awake()
    {
        Cursor.visible = true;
		bodyAnim = body.GetComponent<Animator> ();
        legsAnim = legs.GetComponent<Animator> ();
        gunAnim = gun.GetComponent<Animator> ();
    }
	void Update () 
	{
        ChangeMovement();
        ChangeRotation();

        if(Input.GetMouseButtonDown(0))
        {
            ShotBullet();
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
    void ShotBullet()
    {
        Instantiate(bullet, gun.transform.position, Quaternion.Euler(0 ,0f, aux.transform.eulerAngles.z + 90f));
    }
}
