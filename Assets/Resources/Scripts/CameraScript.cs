using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    float speed;
    [SerializeField]
    Transform playerPosition;
    [SerializeField]
    GameObject door;
    [SerializeField]
    Transform[] cameraBorders;
    [SerializeField]
    AnimationClip doorClosingAnimation;

    [HideInInspector]
    public bool lookingAtClosedDoor = false;

    void Update ()
    {
        if ((playerPosition.position.y > cameraBorders[0].position.y || playerPosition.position.x < cameraBorders[1].position.x || playerPosition.position.x > cameraBorders[2].position.x || playerPosition.position.y < cameraBorders[3].position.y) && !lookingAtClosedDoor)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPosition.position.x + offset.x, playerPosition.position.y + offset.y, -14f), speed * Time.deltaTime);
        }
    }

    public IEnumerator MovingCamera(GameObject door, GameObject[] roomColliders)
    {
        playerPosition.gameObject.GetComponent<PlayerBehaviour>().playerCanMove = false;
        lookingAtClosedDoor = true;
        while (Vector3.Distance(new Vector3(door.transform.position.x + offset.x, door.transform.position.y + offset.y, -14f), transform.position) > 0.1f)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(door.transform.position.x + offset.x, door.transform.position.y + offset.y, -14f), 20 * Time.deltaTime);
            yield return new WaitForSecondsRealtime(0.00000000000000000000001f);
        }
        yield return new WaitForSecondsRealtime(0.15f);
        door.GetComponent<Animator>().SetBool("Closing", true);
        yield return new WaitForSecondsRealtime(doorClosingAnimation.length);
        while (Vector3.Distance(playerPosition.transform.position, new Vector3(transform.position.x, transform.position.y, 0)) > 0.1f)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPosition.position.x + offset.x, playerPosition.position.y + offset.y, -14f), 20 * Time.deltaTime);
            yield return new WaitForSecondsRealtime(0.00000000000000000000001f);
        }
        yield return new WaitForSecondsRealtime(0.15f);
        lookingAtClosedDoor = false;
        playerPosition.gameObject.GetComponent<PlayerBehaviour>().playerCanMove = true;
        foreach(GameObject r in roomColliders)
            r.GetComponent<BoxCollider2D>().enabled = true;
    }

    public IEnumerator OpeningDoor(GameObject door)
    {
        lookingAtClosedDoor = true;
        playerPosition.gameObject.GetComponent<PlayerBehaviour>().playerCanMove = false;
        while (Vector3.Distance(new Vector3(door.transform.position.x + offset.x, door.transform.position.y + offset.y, -14f), transform.position) > 0.1f)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(door.transform.position.x + offset.x, door.transform.position.y + offset.y, -14f), 20 * Time.deltaTime);
            yield return new WaitForSecondsRealtime(0.00000000000000000000001f);
        }
        yield return new WaitForSecondsRealtime(0.15f);
        door.GetComponent<Animator>().SetBool("Closing", false);
        yield return new WaitForSecondsRealtime(doorClosingAnimation.length);
        while (Vector3.Distance(playerPosition.transform.position, new Vector3(transform.position.x, transform.position.y, 0)) > 0.1f)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPosition.position.x + offset.x, playerPosition.position.y + offset.y, -14f), 20 * Time.deltaTime);
            yield return new WaitForSecondsRealtime(0.00000000000000000000001f);
        }
        yield return new WaitForSecondsRealtime(0.15f);
        lookingAtClosedDoor = false;
        playerPosition.gameObject.GetComponent<PlayerBehaviour>().playerCanMove = true;
    }

}
