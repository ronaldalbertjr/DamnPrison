using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    [SerializeField]
    Transform[] walkPositions;
    [SerializeField]
    RoomColliderScript roomCollider;
    [SerializeField]
    BoxColliderTriggerScript boxCollider;
    bool fit;
	void Start ()
    {
        fit = false;
        while (!fit)
        {
            int enemyPoints = 0;
            GameObject enemy = (GameObject)boxCollider.enemies[Random.Range(0, boxCollider.enemies.Length)];
            if (enemy.GetComponent<TankScript>())
            {
                enemyPoints = 5;
            }
            else if (enemy.GetComponent<DogScript>())
            {
                enemyPoints = 3;
            }
            else if (enemy.GetComponent<DefaultEnemyScript>())
            {
                enemyPoints = 3;
            }
            else if (enemy.GetComponent<PorreteScript>())
            {
                enemyPoints = 4;
            }
            if (boxCollider.pointsInTheRoom + enemyPoints <= boxCollider.maxPoints)
            {
                boxCollider.pointsInTheRoom += enemyPoints;
                fit = true;
                enemy.GetComponent<EnemyMovementDefaultScript>().walkPositions = walkPositions;
                GameObject newEnemy = Instantiate(enemy, this.transform.position, this.transform.rotation, transform.parent.parent.parent);
                boxCollider.numberOfEnemiesInRoom++;
                roomCollider.enemiesInRoom.Add(newEnemy);
            }
            if (boxCollider.pointsInTheRoom >= boxCollider.maxPoints - 2)
                break;
        }
	}
}
