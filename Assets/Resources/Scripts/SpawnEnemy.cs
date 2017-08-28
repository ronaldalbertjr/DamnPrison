using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    #region Variables
        [SerializeField]
        Transform[] spawnPoints;
        [SerializeField]
        GameObject[] enemy;
    #endregion

    public void Instantiate()
    {
        GameObject gameObject = (GameObject)Instantiate(enemy[Random.Range(0, enemy.Length)], spawnPoints[Mathf.FloorToInt(Random.Range(1, 4))].position, new Quaternion(0, 0, 0, 0));
        if (gameObject.GetComponent<TankScript>())
            gameObject.GetComponent<TankScript>().startingPoint = transform.position;
        if (gameObject.GetComponent<PorreteScript>())
            gameObject.GetComponent<PorreteScript>().startingPoint = transform.position;
        if (gameObject.GetComponent<RangedEnemyScript>())
            gameObject.GetComponent<RangedEnemyScript>().startingPoint = transform.position;
        if (gameObject.GetComponent<DogScript>())
            gameObject.GetComponent<DogScript>().startingPoint = transform.position;
    }
}
