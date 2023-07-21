using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLocation : MonoBehaviour
{
    private GameObject focusTarget;

    private void Start()
    {
        focusTarget = GameObject.Find("Player Cell(Clone)");
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        // Follow the player if they are alive.
        if (focusTarget != null)
        {
            transform.position = new(focusTarget.transform.position.x,
                transform.position.y, focusTarget.transform.position.z);
        }
        else
        {
            focusTarget = GameObject.Find("Player Cell(Clone)");
            if (focusTarget == null )
            {
                GameManager.IsGameOver = true;
            }
        }
    }
}
