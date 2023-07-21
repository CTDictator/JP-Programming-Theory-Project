using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectionBarrier : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cell") || collision.gameObject.CompareTag("Food"))
        {
            collision.gameObject.transform.Rotate(0.0f, collision.gameObject.transform.rotation.y + 90.0f, 0.0f);
        }
    }
}
