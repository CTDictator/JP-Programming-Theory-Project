using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INHERITANCE
public class PlayerCell : SingleCell
{
    //ENCAPSULATION
    private float verticalInput, horizontalInput;

    // Spawn in facing a random direction.
    private void Start()
    {
        SetRandomDirection();
    }

    // Update is called once per frame
    private void Update()
    {
        // Move the player cell.
        MoveCell();
        // Decay the player cells energy.
        ChangeCellEnergy();
        //if (Input.GetKeyDown(KeyCode.Q)) Mitosis();
    }

    // Takes player inputs and moves the cell accordingly.
    // POLYMORPHISM & ABSTRACTION
    protected override void MoveCell()
    {
        // Adjust cell speed based on its energy state.
        cellSpeedModifier = (cellState == EnergyState.low) ? 1.0f : 3.0f;
        // Forward/backward.
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * cellSpeed * cellSpeedModifier);
        // Left/Right.
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * horizontalInput * Time.deltaTime * cellRotation);
        if (horizontalInput != 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * cellSpeed / 3);
        }
        // Holding spacebar enters player into high energy state.
        if (Input.GetKey(KeyCode.Space)) cellState = EnergyState.high;
        else cellState = EnergyState.low;
    }

    // Split the cell into two if conditions are met.
    protected override void Mitosis()
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
}
