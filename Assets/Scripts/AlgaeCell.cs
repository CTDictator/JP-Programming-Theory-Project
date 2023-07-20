using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgaeCell : MeatCell
{
    // Variables to regulate overpopulation.
    private static int populationMax = 100;
    private static int populationCurrent;
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(ResetForces());
        SetRandomDirection();
        populationCurrent++;
    }

    // Update is called once per frame
    private void Update()
    {
        MoveCell();
        ChangeCellEnergy();
    }

    // The algae cell generates cell energy instead of decaying it.
    protected override void ChangeCellEnergy()
    {
        if (cellEnergy < cellEnergyMax) cellEnergy += Time.deltaTime * energyRate;
        // Once the algae is topped up on energy, perform mitosis.
        if (cellEnergy > cellEnergyMax && populationCurrent <= populationMax)
        {
            cellEnergy = 0.0f;
            Mitosis();
        }
    }

    // Detract the population on death.
    private void OnDestroy()
    {
        populationCurrent--;
    }

    protected override void Mitosis()
    {
        StartCoroutine(ResetForces());
        Instantiate(gameObject, transform.position, transform.rotation);
    }
}
