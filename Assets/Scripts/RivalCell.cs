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
        // Move the rival cell.
        MoveCell();
        // Decay the rival cells energy.
        ChangeCellEnergy();
    }

    // After a little bit of time, continue to keep track of target if within range.
    private IEnumerator ForgetTarget()
    {
        yield return new WaitForSeconds(2);
        // <-
        target = null;
    }

    protected override void MoveCell()
    {
        // Move forward.
        transform.Translate(Vector3.forward * Time.deltaTime * cellSpeed);

        // If it's spotted a target, decided whether to flee the target or pursue it.
        if (target != null)
        {
            // Start a countdown for a memory check of the target.
            StartCoroutine(ForgetTarget());
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
