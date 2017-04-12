using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    AnimationClip doorClosingAnimation;
    GameObject cam;
    GameObject gameManager;
    GameObject player;
    public bool doorClosing;
    [SerializeField]
    string doorPos;

    Animator anim;
    private void Awake()
    {
        cam = Camera.main.gameObject;
    }
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager");
	}
	
	void Update ()
    {
        if (doorClosing)
        {
            StartCoroutine(DoorClosing());
            doorClosing = false;
        }
	}

    IEnumerator DoorClosing()
    {
        cam.GetComponent<CameraScript>().lookingAtClosedDoor = true;
        anim.SetBool("Closing", true);
        yield return new WaitForSeconds(doorClosingAnimation.length);
        cam.GetComponent<CameraScript>().lookingAtClosedDoor = false;
    }
    public void ChangeRoom()
	{
		if(doorPos.Equals("Up") && gameManager.GetComponent<ProceduralScripting>().lin > 0)
        {
			gameManager.GetComponent<ProceduralScripting>().lin--;
			Vector3 newPos = gameManager.GetComponent<ProceduralScripting>().g[gameManager.GetComponent<ProceduralScripting>().lin, gameManager.GetComponent<ProceduralScripting>().col].transform.FindChild("playerPositionFromDown").transform.position;
            player.transform.position = newPos;
            cam.transform.position = player.transform.position;
        }
		else if(doorPos.Equals("Down") && gameManager.GetComponent<ProceduralScripting>().lin < gameManager.GetComponent<ProceduralScripting>().g.GetLength(0) - 1)
        {
			gameManager.GetComponent<ProceduralScripting>().lin++;
			Vector3 newPos = gameManager.GetComponent<ProceduralScripting>().g[gameManager.GetComponent<ProceduralScripting>().lin, gameManager.GetComponent<ProceduralScripting>().col].transform.FindChild("playerPositionFromUp").transform.position;
            player.transform.position = newPos;
            cam.transform.position = player.transform.position;
        }
		else if(doorPos.Equals("Left") && gameManager.GetComponent<ProceduralScripting>().col > 0)
        {
			gameManager.GetComponent<ProceduralScripting> ().col--;
			Vector3 newPos = gameManager.GetComponent<ProceduralScripting>().g[gameManager.GetComponent<ProceduralScripting>().lin, gameManager.GetComponent<ProceduralScripting>().col].transform.FindChild("playerPositionFromRight").transform.position;
            player.transform.position = newPos;
            cam.transform.position = player.transform.position;
        }
		else if(doorPos.Equals("Right") && gameManager.GetComponent<ProceduralScripting>().col < gameManager.GetComponent<ProceduralScripting>().g.GetLength(1) - 1)
        {
			gameManager.GetComponent<ProceduralScripting> ().col++;
			Vector3 newPos = gameManager.GetComponent<ProceduralScripting>().g[gameManager.GetComponent<ProceduralScripting>().lin, gameManager.GetComponent<ProceduralScripting>().col].transform.FindChild("playerPositionFromLeft").transform.position;
            player.transform.position = newPos;
            cam.transform.position = player.transform.position;
        }
		player.GetComponent<PlayerBehaviour>().currentRoom = gameManager.GetComponent<ProceduralScripting>().g[gameManager.GetComponent<ProceduralScripting>().lin, gameManager.GetComponent<ProceduralScripting>().col];

    }
}
