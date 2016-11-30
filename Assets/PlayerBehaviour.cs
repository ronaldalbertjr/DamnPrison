using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour 
{
    [SerializeField]
    float speed;
    Rigidbody2D rb;
	Animator anim;
    Vector3 mousePosition;
    Vector3 point;
    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
		anim = this.GetComponent<Animator> ();
    }
	void Update () 
	{
        mousePosition = Input.mousePosition;
		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.D)) 
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
			anim.SetBool("WalkingUp", true);
			anim.SetBool("WalkingLeft", false);
			anim.SetBool("WalkingDown", false);
			anim.SetBool("WalkingRight", false);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * speed * Time.deltaTime;
			anim.SetBool("WalkingLeft", true);
			anim.SetBool("WalkingUp", false);
			anim.SetBool("WalkingRight", false);
			anim.SetBool("WalkingDown", true);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += Vector3.left * speed * Time.deltaTime;
			anim.SetBool("WalkingRight", true);
			anim.SetBool("WalkingUp", false);
			anim.SetBool("WalkingLeft", false);
			anim.SetBool("WalkingDown", false);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += Vector3.down * speed * Time.deltaTime;
			anim.SetBool("WalkingDown", true);
			anim.SetBool("WalkingUp", false);
			anim.SetBool("WalkingLeft", false);
			anim.SetBool("WalkingRight", false);
        }
        
	}
}
