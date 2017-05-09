using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    GameObject enemy;

    float time;
    void Start()
    {
        time = 6;
    }
	void Update ()
    {
        time += Time.deltaTime;

        if(time >= 5)
        {
            time = 0;
            Instantiate(enemy, spawnPoints[Mathf.FloorToInt(Random.Range(1, 4))].position, new Quaternion(0,0,0,0));
        }
		
	}
}
