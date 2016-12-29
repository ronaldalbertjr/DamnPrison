using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    float speed;
	void Update ()
    {
        this.transform.position += transform.right * Time.deltaTime * speed; 
	}
}
