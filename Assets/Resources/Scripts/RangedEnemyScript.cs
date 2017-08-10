using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RangedEnemyScript : MonoBehaviour
{
    #region Variables
    GameObject spawner;
    GameObject bullet;
    GameObject player;
    GameObject aux;
    Animator anim;
    [HideInInspector]
    public BoxColliderTriggerScript boxColliderTrigger;
    float time;
    float timeToMove;
    float angle;
    float health;
    bool walking;
    bool canShoot = true;
    [HideInInspector]
    public bool isDead;
    #endregion

    void Start ()
    {
        boxColliderTrigger = transform.parent.FindChild("BoxColliderTrigger").GetComponent<BoxColliderTriggerScript>();
        health = 5;
        aux = transform.FindChild("AuxRotationEnemy").gameObject;
        aux.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        aux.GetComponent<EnemyArmaScript>().pointPosition = player.transform.transform;
        bullet = (GameObject)Resources.Load("Prefabs/Weapons/enemyBullet", typeof(GameObject));
        GetComponent<PolyNavAgent>().OnDestinationReached += setCanShootTrue;
    }

    void Update()
    {
        if (!isDead)
        {
            if (canShoot)
            {
                GetComponent<PolyNavAgent>().Stop();
                canShoot = CheckShot(transform.position);
                OtherChangeRotation(aux, anim);
                anim.SetBool("Walking", false);
                time += Time.deltaTime;
                timeToMove += Time.deltaTime;
                if (time >= 1.5f)
                {
                    time = 0;
                    ShotBullet();
                }
                if (timeToMove >= 5)
                {
                    timeToMove = 0;
                    FindPositionToShoot();
                }
            }
            else if (!GetComponent<PolyNavAgent>().hasPath)
            {
                FindPositionToShoot();
            }
        }
    }

    public void Damaged(GameObject tank = null, bool hittenByTank = false)
    {
        health--;
        StartCoroutine(Shot(hittenByTank, tank));
        if (health <= 0)
        {
            GetComponent<PolyNavAgent>().Stop();
            anim.SetTrigger("Die");
            isDead = true;
            Destroy(GetComponent<Collider2D>());
            Time.timeScale = 1;
            boxColliderTrigger.numberOfEnemiesInRoom--;
            GetComponent<SpriteRenderer>().sortingLayerName = "DeadEnemy";
        }
    }

    public void ShotBullet()
    {
        Instantiate(bullet, this.transform.position, Quaternion.Euler(0, 0f, aux.transform.eulerAngles.z + 90f));
    }

    public void ChangeRotation(Animator anim)
    {
        Vector3 goingPosition = GetComponent<PolyNavAgent>().movingDirection;
        if(Mathf.Round(goingPosition.x) != 0 && Mathf.Round(goingPosition.y) != 0)
        {
            if(goingPosition.x > 0 && goingPosition.y > 0)
            {
                anim.SetFloat("EnemyWalkingFloat", 5);
            }
            else if(goingPosition.x < 0 && goingPosition.y > 0)
            {
                anim.SetFloat("EnemyWalkingFloat", 4);
            }
            else if(goingPosition.x > 0 && goingPosition.y < 0)
            {
                anim.SetFloat("EnemyWalkingFloat", 7);
            }
            else if(goingPosition.x < 0 && goingPosition.y < 0)
            {
                anim.SetFloat("EnemyWalkingFloat", 6);
            }
        }
        else if(Mathf.Round(goingPosition.x) != 0  && Mathf.Round(goingPosition.y) == 0)
        {
            if(goingPosition.x > 0)
            {
                anim.SetFloat("EnemyWalkingFloat", 3);
            }
            else if(goingPosition.x < 0)
            {
                anim.SetFloat("EnemyWalkingFloat", 1);
            }
        }
        else if(Mathf.Round(goingPosition.x) == 0 && Mathf.Round(goingPosition.y) != 0)
        {
            if(goingPosition.y > 0)
            {
                anim.SetFloat("EnemyWalkingFloat", 2);
            }
            else if(goingPosition.y < 0)
            {
                anim.SetFloat("EnemyWalkingFloat", 0);
            }
        }
    }

    public void OtherChangeRotation(GameObject aux, Animator anim)
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

    void FindPositionToShoot()
    {
        bool canGo = false;
        canShoot = false;
        Vector3 newPosition = new Vector3(transform.position.x + UnityEngine.Random.Range(-20, 20), transform.position.y + UnityEngine.Random.Range(-20, 20));
        while(!canGo)
        {
            if(boxColliderTrigger.GetComponent<Collider2D>().bounds.Contains(newPosition) && Vector3.Distance(player.transform.position, newPosition) > 10f)
            {
                //canGo = CheckShot(newPosition);
                canGo = true;
            }
            else
            {
                newPosition = new Vector3(transform.position.x + UnityEngine.Random.Range(-20, 20), transform.position.y + UnityEngine.Random.Range(-20, 20));
            }
        }
        GetComponent<PolyNavAgent>().SetDestination(newPosition);
        anim.SetBool("Walking", true);
    }

    void setCanShootTrue()
    {
        canShoot = true;
    }

    bool CheckShot(Vector3 position)
    {
        Vector3 checkDirection = player.transform.position - position;
        bool reachedPlayer;
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit2D hit = Physics2D.Raycast(position, checkDirection, Mathf.Infinity, layerMask);
        if (hit.collider.tag.Equals("Player"))
            reachedPlayer = true;
        else
            reachedPlayer = false;
        return reachedPlayer;
    }

    IEnumerator Shot(bool hittenByTank, GameObject tank)
    {
        anim.SetBool("Shot", true);
        if (hittenByTank)
            StartCoroutine(HittenByTank(tank));
        else
        {
            int aux = Convert.ToInt32(anim.GetFloat("EnemyWalkingFloat"));
            Vector3 pos = player.transform.position;
            StartCoroutine(GoingBackWhenShot(aux, pos));
        }
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(ChangeTimeScale());
        StartCoroutine(ChangeEnemyColor());
    }

    IEnumerator ChangeEnemyColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.3f, 0, 0);
        yield return new WaitForSeconds(0.05f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        yield return 0;
    }

    IEnumerator ChangeTimeScale()
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(0.03f); ;
        Time.timeScale = 1;
    }

    IEnumerator HittenByTank(GameObject tank)
    {
        Vector3 auxVector = new Vector3(0, 0);
        if(tank.transform.position.x < transform.position.x)
        {
            if(tank.transform.position.y < transform.position.y)
            {
                auxVector = new Vector3(1F, 1F);
            }
            else if(tank.transform.position.y > transform.position.y)
            {
                auxVector = new Vector3(1F, -1F);
            }
        }
        else if(tank.transform.position.x > transform.position.x)
        {
            if (tank.transform.position.y < transform.position.y)
            {
                auxVector = new Vector3(-1F, 1F);
            }
            else if (tank.transform.position.y > transform.position.y)
            {
                auxVector = new Vector3(-1F, -1F);
            }
        }
        Vector3 diffVector = tank.GetComponent<TankScript>().differenceVector.normalized;
        Vector3 toSum = diffVector + auxVector ;
        for (int i = 0; i < 10; i++)
        {
            transform.position += toSum * 0.25f;
            yield return new WaitForSeconds(0.001f);
        }
        anim.SetBool("Shot", false);
    }

    IEnumerator GoingBackWhenShot(int aux, Vector3 position)
    {
        for (float i = 0; i < 10; i++)
        {
            switch (aux)
            {
                case 0:
                    transform.position += new Vector3(0f, 0.1f, 0f);
                    if (position.x < transform.position.x)
                        anim.SetFloat("EnemyShotFloat", 1);
                    else
                        anim.SetFloat("EnemyShotFloat", 0);
                    break;
                case 1:
                    transform.position += new Vector3(0.1f, 0f, 0f);
                    anim.SetFloat("EnemyShotFloat", 1);
                    break;
                case 2:
                    transform.position += new Vector3(0f, -0.1f, 0f);
                    if (position.x < transform.position.x)
                        anim.SetFloat("EnemyShotFloat", 1);
                    else
                        anim.SetFloat("EnemyShotFloat", 0);
                    break;
                case 3:
                    transform.position += new Vector3(-0.1f, 0f, 0f);
                    anim.SetFloat("EnemyShotFloat", 0);
                    break;
                case 4:
                    transform.position += new Vector3(0.1f, -0.1f, 0f);
                    anim.SetFloat("EnemyShotFloat", 1);
                    break;
                case 5:
                    transform.position += new Vector3(-0.1f, -0.1f, 0f);
                    anim.SetFloat("EnemyShotFloat", 0);
                    break;
                case 6:
                    transform.position += new Vector3(0.1f, 0.1f, 0f);
                    anim.SetFloat("EnemyShotFloat", 1);
                    break;
                case 7:
                    transform.position += new Vector3(-0.1f, 0.1f, 0f);
                    anim.SetFloat("EnemyShotFloat", 0);
                    break;
            }
            yield return new WaitForSeconds(0.001f);
        }
        anim.SetBool("Shot", false);
    }

    IEnumerator ChangeWalkingDirection(GameObject enemy)
    {
        Vector3 differenceVector = transform.position - enemy.transform.position;
        if (Mathf.Abs(differenceVector.x) < Mathf.Abs(differenceVector.y))
        {
            if (transform.position.x < enemy.transform.position.x)
            {
                for (int i = 0; i < 10; i++)
                {
                    transform.position += new Vector3(-1, 0) * 0.005f;
                    yield return new WaitForSeconds(0.001f);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    transform.position += new Vector3(1, 0) * 0.005f;
                    yield return new WaitForSeconds(0.001f);
                }
            }
        }
        else
        {
            if (transform.position.y < enemy.transform.position.y)
            {
                for (int i = 0; i < 10; i++)
                {
                    transform.position += new Vector3(0, -1) * 0.005f;
                    yield return new WaitForSeconds(0.001f);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    transform.position += new Vector3(0, 1) * 0.005f;
                    yield return new WaitForSeconds(0.001f);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (GetComponent<PolyNavAgent>().hasPath)
        {
            canShoot = true;
        }
    }

}
