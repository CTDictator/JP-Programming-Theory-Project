using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLocation : MonoBehaviour
{
    private GameObject focusTarget;
    // Start is called before the first frame update
    void Start()
    {
        // Set the focus location on the player at the start.
        focusTarget = GameObject.Find("Player Cell");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Follow the player if they are alive.
        if (focusTarget != null)
        {
            transform.position = new(focusTarget.transform.position.x,
                transform.position.y, focusTarget.transform.position.z);
        }
    }
}
