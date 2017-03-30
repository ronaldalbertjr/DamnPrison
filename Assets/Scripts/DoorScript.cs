using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]
    AnimationClip doorClosingAnimation;
    [SerializeField]
    GameObject cam;
    public bool doorClosing;

    Animator anim;
	void Start ()
    {
        anim = GetComponent<Animator>();
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
}
