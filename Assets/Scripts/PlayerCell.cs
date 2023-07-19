using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INHERITANCE
public class PlayerCell : SingleCell
{
    //ENCAPSULATION
    private float verticalInput, horizontalInput;

    // Update is called once per frame
    private void Update()
    {
        // Move the player cell.
        MoveCell();
        // Decay the player cells energy.
        ChangeCellEnergy();
    }

    // Takes player inputs and moves the cell accordingly.
    // POLYMORPHISM & ABSTRACTION
    protected override void MoveCell()
    {
        // Forward/backward.
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * cellSpeed);
        // Left/Right.
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * horizontalInput * Time.deltaTime * cellRotation);
        if (horizontalInput != 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * cellSpeed / 3);
        }
    }
}
