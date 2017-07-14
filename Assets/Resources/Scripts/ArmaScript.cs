using UnityEngine;
using System.Collections;

public class ArmaScript : MonoBehaviour
{
    #region Variables
        Vector3 mousePosition;
        Vector3 lookPosition;
        Quaternion rotation;
    #endregion

	void Update ()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float AngleRad = Mathf.Atan2(-mousePosition.x + transform.position.x, mousePosition.y - transform.position.y);
        float angle = (180 / Mathf.PI) * AngleRad;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
