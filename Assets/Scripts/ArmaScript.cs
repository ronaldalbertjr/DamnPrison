using UnityEngine;
using System.Collections;

public class ArmaScript : MonoBehaviour
{
    Vector3 mousePosition;
    Vector3 lookPosition;
    Quaternion rotation;
	void Update ()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookPosition = mousePosition - this.transform.position;
        rotation = Quaternion.LookRotation(lookPosition);
        rotation = new Quaternion(0f, 0f, rotation.z / 2, rotation.w / 2);
        this.transform.rotation = rotation;
    }
}
