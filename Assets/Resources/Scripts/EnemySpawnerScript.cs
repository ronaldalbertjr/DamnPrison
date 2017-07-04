using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    #region Variables
        [SerializeField]
        Transform[] walkPositions;
        [SerializeField]
        RoomColliderScript roomCollider;
        [SerializeField]
        BoxColliderTriggerScript boxCollider;
        bool fit;
    #endregion

    void Start ()
    {
        fit = false;
        while (!fit)
        {
            int enemyPoints = 0;
            GameObject enemy = (GameObject)boxCollider.enemies[Random.Range(0, boxCollider.enemies.Length)];
            if (enemy.GetComponent<TankScript>())
            {
                if (boxCollider.thereIsAlreadyATank)
                {
                    continue;
                }
                enemyPoints = 5;
                enemy.GetComponent<TankScript>().boxColliderTrigger = boxCollider;
                boxCollider.thereIsAlreadyATank = true; 
            }
            else if (enemy.GetComponent<DogScript>())
            {
                enemyPoints = 3;
                enemy.GetComponent<DogScript>().boxColliderTrigger = boxCollider;
            }
            else if (enemy.GetComponent<RangedEnemyScript>())
            {
                enemyPoints = 3;
                enemy.GetComponent<RangedEnemyScript>().boxColliderTrigger = boxCollider;
            }
            else if (enemy.GetComponent<PorreteScript>())
            {
                enemyPoints = 4;
                enemy.GetComponent<PorreteScript>().boxColliderTrigger = boxCollider;
            }
            if (boxCollider.pointsInTheRoom >= boxCollider.maxPoints - 2)
                break;
            else if (boxCollider.pointsInTheRoom + enemyPoints <= boxCollider.maxPoints)
            {
                boxCollider.pointsInTheRoom += enemyPoints;
                fit = true;
                enemy.GetComponent<EnemyMovementDefaultScript>().walkPositions = walkPositions;
                GameObject newEnemy = Instantiate(enemy, this.transform.position, this.transform.rotation, transform.parent.parent.parent);
                boxCollider.numberOfEnemiesInRoom++;
                roomCollider.enemiesInRoom.Add(newEnemy);
            }
        }
	}
}
