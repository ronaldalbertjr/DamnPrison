using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    float speed;

    float time;
	void Update ()
    {
        time += Time.deltaTime;
        this.transform.position += transform.right * Time.deltaTime * speed; 

        if(time >= 10)
        {
            Destroy(gameObject);
        }
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("EnemyDefault"))
        {
            col.GetComponent<DefaultEnemyScript>().Damaged();
            Destroy(gameObject);
        }
    }
}
