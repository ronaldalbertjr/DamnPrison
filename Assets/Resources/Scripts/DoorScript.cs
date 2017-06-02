using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    GameObject cam;
    GameObject gameManager;
    GameObject player;
    BoxColliderTriggerScript boxColliderTrigger;
    public bool doorClosing;
    [SerializeField]
    string doorPos;

    private void Awake()
    {
        cam = Camera.main.gameObject;
        boxColliderTrigger = transform.parent.FindChild("BoxColliderTrigger").GetComponent<BoxColliderTriggerScript>();
    }
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.Find("GameManager");
	}

    public void ChangeRoom()
    {
        if (boxColliderTrigger.numberOfEnemiesInRoom <= 0)
        {
            if (doorPos.Equals("Up") && gameManager.GetComponent<ProceduralScripting>().lin > 0)
            {
                gameManager.GetComponent<ProceduralScripting>().lin--;
                Vector3 newPos = gameManager.GetComponent<ProceduralScripting>().g[gameManager.GetComponent<ProceduralScripting>().lin, gameManager.GetComponent<ProceduralScripting>().col].transform.FindChild("playerPositionFromDown").transform.position;
                player.transform.position = newPos;
                cam.transform.position = newPos - new Vector3(0f, 0f, 14f);
            }
            else if (doorPos.Equals("Down") && gameManager.GetComponent<ProceduralScripting>().lin < gameManager.GetComponent<ProceduralScripting>().g.GetLength(0) - 1)
            {
                gameManager.GetComponent<ProceduralScripting>().lin++;
                Vector3 newPos = gameManager.GetComponent<ProceduralScripting>().g[gameManager.GetComponent<ProceduralScripting>().lin, gameManager.GetComponent<ProceduralScripting>().col].transform.FindChild("playerPositionFromUp").transform.position;
                player.transform.position = newPos;
                cam.transform.position = newPos - new Vector3(0f, 0f, 14f);
            }
            else if (doorPos.Equals("Left") && gameManager.GetComponent<ProceduralScripting>().col > 0)
            {
                gameManager.GetComponent<ProceduralScripting>().col--;
                Vector3 newPos = gameManager.GetComponent<ProceduralScripting>().g[gameManager.GetComponent<ProceduralScripting>().lin, gameManager.GetComponent<ProceduralScripting>().col].transform.FindChild("playerPositionFromRight").transform.position;
                player.transform.position = newPos;
                cam.transform.position = newPos - new Vector3(0f, 0f, 14f);
            }
            else if (doorPos.Equals("Right") && gameManager.GetComponent<ProceduralScripting>().col < gameManager.GetComponent<ProceduralScripting>().g.GetLength(1) - 1)
            {
                gameManager.GetComponent<ProceduralScripting>().col++;
                Vector3 newPos = gameManager.GetComponent<ProceduralScripting>().g[gameManager.GetComponent<ProceduralScripting>().lin, gameManager.GetComponent<ProceduralScripting>().col].transform.FindChild("playerPositionFromLeft").transform.position;
                player.transform.position = newPos;
                cam.transform.position = newPos - new Vector3(0f, 0f, 14f);
            }
            player.GetComponent<PlayerBehaviour>().currentRoom = gameManager.GetComponent<ProceduralScripting>().g[gameManager.GetComponent<ProceduralScripting>().lin, gameManager.GetComponent<ProceduralScripting>().col];
        }
    }
}
