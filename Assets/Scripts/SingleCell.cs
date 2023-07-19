using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class SingleCell : MonoBehaviour
{
    [SerializeField] protected float cellSpeed;
    [SerializeField] protected float cellRotation;
    [SerializeField] protected int cellStrength;

    // ENCAPSULATION
    public int Strength
    {
        get { return cellStrength; }
    }

    protected abstract void MoveCell();

    // If the cell collides with a weaker cell, destroy it.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cell") && 
            cellStrength > collision.gameObject.GetComponentInParent<SingleCell>().Strength)
        {
            // Devour the target.
            Destroy(collision.gameObject);
        }
    }
}
