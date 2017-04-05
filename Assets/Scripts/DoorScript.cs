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
        int lin;
        int col;
        for(int i = 0; i < gameManager.GetComponent<ProceduralScripting>().grid.GetLength(0); i++)
        {
            for(int j = 0; j < gameManager.GetComponent<ProceduralScripting>().grid.GetLength(1); j++)
            {
                if (gameManager.GetComponent<ProceduralScripting>().grid[i,j].Equals(player.GetComponent<PlayerBehaviour>().currentRoom))
                {
                    lin = i;
                    col = j;
                }
            }
        }

        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow))
        {

        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow))
        {

        }
        else if(Input.GetKeyUp(KeyCode.RightArrow))
        {

        }
    }
}
