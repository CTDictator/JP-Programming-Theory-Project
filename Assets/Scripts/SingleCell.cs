using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class SingleCell : MonoBehaviour
{
    public GameObject deathObject;
    protected enum EnergyState { low, high, max_energy_state };
    protected static float energyRateMultiplier = 10.0f;
    protected static float cellGrowthDivider = 0.7f;
    protected static float cellMaxEnergyIncrease = 10.0f;
    protected static float cellEnergyRateIncrease = -0.3f;
    protected static float cellSpeedIncrease = 0.5f;
    protected static float cellSizeIncrease = 0.1f;
    protected static float maxAngle = 360.0f;
    protected static int minMitosisSize = 2;

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

    public float Energy
    {
        get { return cellEnergy; }
    }

    protected abstract void MoveCell();

    // Changes the energy level of the cell over time.
    protected virtual void ChangeCellEnergy()
    {
        // Burn energy a lot quicker in high energy cell state.
        cellEnergy += Time.deltaTime * energyRate * 
            (cellState == EnergyState.high ? energyRateMultiplier : 1.0f);
        // Split apart a cell into meat chunks on starvation.
        if (cellEnergy < 0)
        {
            if (deathObject != null)
            {
                for (int i = 0; i < cellStrength; i++)
                {
                    Instantiate(deathObject, transform.position, transform.rotation);
                }
            }
            Destroy(gameObject);
        }
    }

    // If the cell collides with a weaker cell, destroy it.
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Cell") || collision.gameObject.CompareTag("Food")) &&
            cellStrength > collision.gameObject.GetComponent<SingleCell>().Strength)
        {
            // Absorb the energy of the weaker cell.
            cellEnergy += collision.gameObject.GetComponent<SingleCell>().Energy;
            // Check to see if the cell grows in strength and size.
            AssessCellGrowth();
            // Destroy the weaker cell.
            Destroy(collision.gameObject);
        }
    }

    // Assess if the cell grows and by how much.
    protected virtual void AssessCellGrowth()
    {
        if (cellEnergy > cellEnergyMax)
        {
            // Raise the strength and size of the cell by 1 level.
            ++cellStrength;
            // Reduce the cells energy due to growth.
            cellEnergy *= cellGrowthDivider;
            // Raise the maximum energy of the cell and energy rate consumption.
            cellEnergyMax += cellMaxEnergyIncrease;
            energyRate += cellEnergyRateIncrease;
            // Make the cell a little faster.
            cellSpeed += cellSpeedIncrease;
            // Make the cell physically bigger.
            transform.localScale += Vector3.one * cellSizeIncrease;
        }
    }

    // Set a new cell to face a new direction.
    protected void SetRandomDirection()
    {
        transform.Rotate(0.0f, Random.Range(0.0f, maxAngle), 0.0f);
    }

    // Split the cell into two if conditions are met.
    protected virtual void Mitosis()
    {
        // Has to be at least minimum size to split.
        if (cellStrength >= minMitosisSize)
        {
            StartCoroutine(ResetForces());
            // Calculate how many levels the cell is dropping.
            int dropInCellStrength = cellStrength - (cellStrength / minMitosisSize);
            cellStrength /= minMitosisSize;
            cellEnergy /= minMitosisSize;
            // Lower the cells stats respectively.
            for (int i = 0; i < dropInCellStrength; ++i)
            {
                cellEnergyMax -= cellMaxEnergyIncrease;
                energyRate -= cellEnergyRateIncrease;
                cellSpeed -= cellSpeedIncrease;
                transform.localScale -= Vector3.one * cellSizeIncrease;
            }
            // Make the copy.
            Instantiate(gameObject, transform.position, transform.rotation);
        }
    }

    // Reset the collosal force generated when a cell dies or performs mitosis.
    protected IEnumerator ResetForces()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
