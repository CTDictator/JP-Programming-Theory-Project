using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class SingleCell : MonoBehaviour
{
    protected enum EnergyState { low, high, max_energy_state };
    protected const float energyRateMultiplier = 10.0f;

    [SerializeField] protected EnergyState cellState;
    [SerializeField] protected int cellStrength;
    [SerializeField] protected float cellEnergy;
    [SerializeField] protected float cellEnergyMax;
    [SerializeField] protected float energyRate;
    [SerializeField] protected float cellSpeed;
    [SerializeField] protected float cellSpeedModifier;
    [SerializeField] protected float cellRotation;

    // ENCAPSULATION
    public int Strength
    {
        get { return cellStrength; }
    }

    protected abstract void MoveCell();

    // Changes the energy level of the cell over time.
    protected void ChangeCellEnergy()
    {
        // Burn energy a lot quicker in high energy cell state.
        cellEnergy += Time.deltaTime * energyRate * 
            (cellState == EnergyState.high ? energyRateMultiplier : 1.0f);
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
