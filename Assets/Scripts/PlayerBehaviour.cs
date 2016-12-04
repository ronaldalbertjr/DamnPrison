using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour 
{
    [SerializeField]
    float speed;
    [SerializeField]
    GameObject aux;


    Rigidbody2D rb;
	Animator anim;
    float angle;


    void Awake()
    {
		anim = this.GetComponent<Animator> ();
    }
	void Update () 
	{
        ChangeMovement();
        ChangeRotation();
	}

    void ChangeMovement()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            this.transform.position += (Vector3.up + Vector3.right) * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            this.transform.position += (Vector3.up + Vector3.left) * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            this.transform.position += (Vector3.down + Vector3.right) * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            this.transform.position += (Vector3.down + Vector3.left) * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += Vector3.up * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += Vector3.down * speed * Time.deltaTime;
        }
    }
    void ChangeRotation()
    {
        angle = aux.transform.eulerAngles.z;
        if (angle > 157.5 && angle < 202.5)
        {
            anim.SetFloat("WalkingFloat", 0);
        }
        else if (angle > 247.5 && angle < 292.5)
        {
            anim.SetFloat("WalkingFloat", 3);
        }
        else if (angle > 67.5 && angle < 112.5)
        {
            anim.SetFloat("WalkingFloat", 1);
        }
        else if(angle >= 22.5 && angle <= 67.5)
        {
            anim.SetFloat("WalkingFloat", 5);
        }
        else if(angle >= 292.5 && angle <= 337.5)
        {
            anim.SetFloat("WalkingFloat", 4);
        }
        else if(angle >= 112.5 && angle <= 157.5)
        {
            anim.SetFloat("WalkingFloat", 7);
        }
        else if(angle >= 202.5 && angle <= 247.5)
        {
            anim.SetFloat("WalkingFloat", 6);
        }
        else if (angle < 22.5 || angle > 337.5)
        {
            anim.SetFloat("WalkingFloat", 2);
        }
    }
}
