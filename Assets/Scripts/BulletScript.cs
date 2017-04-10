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
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("EnemyDefault"))
        {
            col.GetComponent<DefaultEnemyScript>().Damaged();
            Destroy(gameObject);
        }
       
        if (col.tag.Equals("Background"))
        {
            Destroy(gameObject);
        }
        
    }
}
