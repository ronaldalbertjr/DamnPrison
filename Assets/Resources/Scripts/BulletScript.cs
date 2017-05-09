using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    string thrownBy; 
    
	void Update ()
    {
        this.transform.position += transform.right * Time.deltaTime * speed; 
        
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("EnemyDefault") && thrownBy.Equals("Player"))
        {
            col.GetComponent<RangedEnemyScript>().Damaged();
            Destroy(gameObject);
        }
        else if (col.tag.Equals("Player") && thrownBy.Equals("Enemy"))
        {
            Debug.Log("playerDamaged");
            Destroy(gameObject);
        }

        else if (col.tag.Equals("Background"))
        {
            Destroy(gameObject);
        }
        
    }
}
