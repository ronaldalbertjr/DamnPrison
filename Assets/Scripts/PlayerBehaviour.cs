using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour 
{
    [SerializeField]
    float speed;
    Rigidbody2D rb;
	Animator anim;
    void Awake()
    {
		anim = this.GetComponent<Animator> ();
    }
	void Update () 
	{
		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.D)) 
		{
            anim.SetFloat("WalkingFloat", 4);
            this.transform.position += (Vector3.up + Vector3.right) * speed * Time.deltaTime;
		}
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            this.transform.position += (Vector3.up + Vector3.left) * speed * Time.deltaTime;
            anim.SetFloat("WalkingFloat", 5);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            this.transform.position += (Vector3.down + Vector3.right) * speed * Time.deltaTime;
            anim.SetFloat("WalkingFloat", 6);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            this.transform.position += (Vector3.down + Vector3.left) * speed * Time.deltaTime;
            anim.SetFloat("WalkingFloat", 7);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += Vector3.up * speed * Time.deltaTime;
            anim.SetFloat("WalkingFloat", 2);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * speed * Time.deltaTime;
            anim.SetFloat("WalkingFloat", 3);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += Vector3.left * speed * Time.deltaTime;
            anim.SetFloat("WalkingFloat", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            anim.SetFloat("WalkingFloat", 0);
            this.transform.position += Vector3.down * speed * Time.deltaTime;
        }
        
	}
}
