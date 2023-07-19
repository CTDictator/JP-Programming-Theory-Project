using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class SingleCell : MonoBehaviour
{
    protected enum EnergyState { low, high, max_energy_state };

    [SerializeField] protected EnergyState cellState;
    [SerializeField] protected int cellStrength;
    [SerializeField] protected float cellEnergy;
    [SerializeField] protected float energyRate;
    [SerializeField] protected float cellSpeed;
    [SerializeField] protected float cellRotation;

    // ENCAPSULATION
    public int Strength
    {
        get { return cellStrength; }
    }

    protected abstract void MoveCell();

    protected void ChangeCellEnergy()
    {
        cellEnergy += Time.deltaTime * energyRate;
        if (cellEnergy < 0) Destroy(gameObject);
    }

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
