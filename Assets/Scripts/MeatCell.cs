using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatCell : SingleCell
{
    // Select a random angle to spawn in.
    private void Start()
    {
        StartCoroutine(ResetForces());
        SetRandomDirection();
    }

    // Slowely creep the cell in one direction.
    private void Update()
    {
        MoveCell();
    }

    // Slowely drift in a direction.
    protected override void MoveCell()
    {
        // float in one direction.
        transform.Translate(Vector3.forward * Time.deltaTime * cellSpeed);
    }
}
