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
    
	Animator bodyAnim;
    Animator legsAnim;
    float angle;


    void Awake()
    {
		bodyAnim = body.GetComponent<Animator> ();
        legsAnim = legs.GetComponent<Animator> ();
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
        }
        else if (angle > 247.5 && angle < 292.5)
        {
            bodyAnim.SetFloat("WalkingBody", 1);
        }
        else if (angle > 67.5 && angle < 112.5)
        {
            bodyAnim.SetFloat("WalkingBody", 3);
        }
        else if(angle >= 22.5 && angle <= 67.5)
        {
            bodyAnim.SetFloat("WalkingBody", 5);
        }
        else if(angle >= 292.5 && angle <= 337.5)
        {
            bodyAnim.SetFloat("WalkingBody", 4);
        }
        else if(angle >= 112.5 && angle <= 157.5)
        {
            bodyAnim.SetFloat("WalkingBody", 7);
        }
        else if(angle >= 202.5 && angle <= 247.5)
        {
            bodyAnim.SetFloat("WalkingBody", 6);
        }
        else if (angle < 22.5 || angle > 337.5)
        {
            bodyAnim.SetFloat("WalkingBody", 2);
        }
    }
}
