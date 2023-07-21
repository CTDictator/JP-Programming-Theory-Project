using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgaeCell : MeatCell
{
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(ResetForces());
        SetRandomDirection();
        GameManager.CurrentAlgae++;
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
        if (cellEnergy > cellEnergyMax && GameManager.CurrentAlgae <= GameManager.MaxAlgae)
        {
            cellEnergy = 0.0f;
            Mitosis();
        }
    }

    // Detract the population on death.
    private void OnDestroy()
    {
        GameManager.CurrentAlgae--;
    }

    protected override void Mitosis()
    {
        StartCoroutine(ResetForces());
        Instantiate(gameObject, transform.position, transform.rotation);
    }
}
