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
        ChangeRoom();
        if(doorClosing)
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
    void ChangeRoom()
    {
        int lin = 0;
        int col = 0;
        for(int i = 0; i < gameManager.GetComponent<ProceduralScripting>().grid.GetLength(0); i++)
        {
            for(int j = 0; j < gameManager.GetComponent<ProceduralScripting>().grid.GetLength(1); j++)
            {
                if (gameManager.GetComponent<ProceduralScripting>().grid[i,j].name.Equals(player.GetComponent<PlayerBehaviour>().currentRoom.name) || gameManager.GetComponent<ProceduralScripting>().grid[i, j].name.Equals(player.GetComponent<PlayerBehaviour>().currentRoom.name+"(Clone)"))
                {
                    lin = i;
                    col = j;
                }
            }
        }
        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            Debug.Log(gameManager.GetComponent<ProceduralScripting>().grid[lin - 1, col].GetComponent<BackgroundScript>().playerPositions[3].transform.position);
            player.transform.position = gameManager.GetComponent<ProceduralScripting>().grid[lin - 1, col].GetComponent<BackgroundScript>().playerPositions[3].transform.position;
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            player.transform.position = gameManager.GetComponent<ProceduralScripting>().grid[lin + 1, col].GetComponent<BackgroundScript>().playerPositions[2].transform.position;
        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            player.transform.position = gameManager.GetComponent<ProceduralScripting>().grid[lin, col - 1].GetComponent<BackgroundScript>().playerPositions[1].transform.position;
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            player.transform.position = gameManager.GetComponent<ProceduralScripting>().grid[lin, col - 1].GetComponent<BackgroundScript>().playerPositions[0].transform.position;
        }
    }
}
