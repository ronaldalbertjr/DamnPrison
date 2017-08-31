using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBossScene : MonoBehaviour
{
    #region Variables
        [SerializeField]
        GameObject[] boxColliderTrigger;
        bool canChangeScene;
    #endregion

    void Start()
    {
        boxColliderTrigger = GameObject.FindGameObjectsWithTag("BoxColliderTrigger");
    }

    void Update ()
    {
        canChangeScene = true;

	    foreach(GameObject i in boxColliderTrigger)
        {
            if(i.GetComponent<BoxColliderTriggerScript>().numberOfEnemiesInRoom > 0)
            {
                canChangeScene = false;
            }
        }

        if(canChangeScene)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
	}
}
