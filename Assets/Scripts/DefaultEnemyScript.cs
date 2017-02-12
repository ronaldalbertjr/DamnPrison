using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemyScript : MonoBehaviour
{
    [SerializeField]
    GameObject aux;
    GameObject player;

    Animator anim;

    float angle;
    float health;
    void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        health = 10;
    }
	void Update ()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.1f);
        ChangeRotation();
        if(health <= 0)
        {
            Destroy(gameObject);
        }

	}
    void ChangeRotation()
    {
        angle = aux.transform.eulerAngles.z;
        if (angle > 157.5 && angle < 202.5)
        {
            anim.SetFloat("EnemyWalkingFloat", 0);
        }
        else if (angle > 247.5 && angle < 292.5)
        {
            anim.SetFloat("EnemyWalkingFloat", 3);
        }
        else if (angle > 67.5 && angle < 112.5)
        {
           anim.SetFloat("EnemyWalkingFloat", 1);
        }
        else if (angle >= 22.5 && angle <= 67.5)
        {
            anim.SetFloat("EnemyWalkingFloat", 4);
        }
        else if (angle >= 292.5 && angle <= 337.5)
        {
            anim.SetFloat("EnemyWalkingFloat", 5);
        }
        else if (angle >= 112.5 && angle <= 157.5)
        {
            anim.SetFloat("EnemyWalkingFloat", 6);
        }
        else if (angle >= 202.5 && angle <= 247.5)
        {
            anim.SetFloat("EnemyWalkingFloat", 7);
        }
        else if (angle < 22.5 || angle > 337.5)
        {
            anim.SetFloat("EnemyWalkingFloat", 2);
        }
    }
    public void Damaged()
    {
        health--;
        StartCoroutine(ChangeEnemyColor());
    }
    IEnumerator ChangeEnemyColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.3f, 0, 0);
        yield return new WaitForSeconds(0.01f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        yield return 0;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag.Equals("Player"))
        {
            Application.Quit();
        }
    }
}
