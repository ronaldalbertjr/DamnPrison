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
        if (col.tag.Equals("EnemyDefault") && thrownBy.Equals("Player") && col.GetComponent<DefaultEnemyScript>().isActiveAndEnabled)
        {
            col.GetComponent<DefaultEnemyScript>().Damaged();
            Destroy(gameObject);
        }
        if (col.tag.Equals("EnemyGun") && thrownBy.Equals("Player") && col.GetComponent<RangedEnemyScript>().isActiveAndEnabled)
        {
            col.GetComponent<RangedEnemyScript>().Damaged();
            Destroy(gameObject);
        }
        else if (col.tag.Equals("Player") && thrownBy.Equals("Enemy"))
        {
            col.gameObject.GetComponent<PlayerBehaviour>().Damaged(col.gameObject);
            Destroy(gameObject);
        }
        else if (col.tag.Equals("Background"))
        {
            Destroy(gameObject);
        }
    }
}
