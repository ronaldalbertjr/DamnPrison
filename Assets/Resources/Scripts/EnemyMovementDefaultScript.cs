using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMovementDefaultScript: MonoBehaviour
{
    public Transform[] walkPositions;
    [SerializeField]
    float speed;
    [HideInInspector]
    public GameObject roomCollider;
    Vector3 currentPosition;
    int counter;
    private void Start()
    {
        currentPosition = walkPositions[counter].position;
        if (GetComponent<RangedEnemyScript>() || GetComponent<TankScript>())
            GetComponent<Animator>().SetBool("Walking", true);
    }
    void Update ()
    {
        ChangeRotation();
        WalkThroughPositions();
	}

    void WalkThroughPositions()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentPosition, Time.deltaTime * speed);
        if(Vector3.Distance(transform.position, currentPosition) < 0.1f)
        {
            counter++;
            if (counter < walkPositions.Length)
                currentPosition = walkPositions[counter].position;
            else
                counter = 0;
                currentPosition = walkPositions[counter].position;
        }
    }

    void ChangeRotation()
    {
        if(GetComponent<DefaultEnemyScript>() || GetComponent<RangedEnemyScript>())
        {
            transform.GetChild(0).gameObject.GetComponent<EnemyArmaScript>().pointPosition = walkPositions[counter];
            if (GetComponent<DefaultEnemyScript>())
                GetComponent<DefaultEnemyScript>().ChangeRotation(transform.GetChild(0).gameObject, GetComponent<Animator>());
            else if (GetComponent<RangedEnemyScript>())
                GetComponent<RangedEnemyScript>().ChangeRotation(transform.GetChild(0).gameObject, GetComponent<Animator>());
        }
        else if(GetComponent<TankScript>())
        {
            GetComponent<TankScript>().Flip(walkPositions[counter], GetComponent<SpriteRenderer>());
        }
        else if(GetComponent<PorreteScript>())
        {
            GetComponent<PorreteScript>().Flip(walkPositions[counter], transform.FindChild("PorreteSpriteRender").gameObject.GetComponent<SpriteRenderer>(), transform.FindChild("PorreteAttackArea").gameObject.GetComponent<BoxCollider2D>());
        }
        else if(GetComponent<DogScript>())
        {
            GetComponent<DogScript>().ChangeAnimation(GetComponent<Animator>());
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag.Equals("Bullet"))
        {
            roomCollider.GetComponent<RoomColliderScript>().RoomEntered();
        }
    }
}
