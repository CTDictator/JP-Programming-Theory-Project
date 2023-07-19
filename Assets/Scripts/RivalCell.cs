using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalCell : SingleCell
{
    private Transform target = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCell();
    }

    protected override void MoveCell()
    {
        // Move forward.
        transform.Translate(Vector3.forward * Time.deltaTime * cellSpeed);

        if (target != null)
        {
            if (cellStrength > target.GetComponent<SingleCell>().Strength)
            {
                FollowTarget();
            }
            else
            {
                FleeTarget();
            }
        }
    }

    // Face away the existential threat.
    private void FleeTarget()
    {
        Quaternion rotTarget = Quaternion.LookRotation(
                new Vector3(-target.position.x, 0.0f, -target.position.z) -
                new Vector3(-transform.position.x, 0.0f, -transform.position.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, cellRotation * Time.deltaTime);
    }

    // Pursue the hapless victim.
    private void FollowTarget()
    {
        Quaternion rotTarget = Quaternion.LookRotation(
                new Vector3(target.position.x, 0.0f, target.position.z) -
                new Vector3(transform.position.x, 0.0f, transform.position.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, cellRotation * Time.deltaTime);
    }

    // If the cell spots another cell, track it.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cell"))
        {
            target = other.gameObject.transform;
        }
    }
}
