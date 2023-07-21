using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RivalCell : SingleCell
{
    [SerializeField] private GameObject target = null;
    private const float trackingDistance = 15.0f;
    private const float closeEnoughAngle = 60.0f;
    private const float cellCriticalFraction = 5.0f;

    // Spawn in facing a random direction.
    private void Start()
    {
        GameManager.CurrentRivals++;
        SetRandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the rival cell.
        MoveCell();
        // Decay the rival cells energy.
        ChangeCellEnergy();
        // Perform Mitosis when the conditions are met.
    }

    // After a little bit of time, continue to keep track of target if within range.
    private IEnumerator ForgetTarget()
    {
        yield return new WaitForSeconds(1);
        // Compare the square magnitude to the square tracking radius.
        if (target != null)
        {
            Vector3 targetDirection = target.transform.position - transform.position;
            float sqrLen = targetDirection.sqrMagnitude;
            if (sqrLen > trackingDistance * trackingDistance)
            {
                cellState = EnergyState.low;
                target = null;
            }
        }
    }

    // Method to dictate how a cell moves.
    protected override void MoveCell()
    {
        // Adjust cell speed based on its energy state.
        cellSpeedModifier = (cellState == EnergyState.low) ? 1.0f : 3.0f;

        // Move forward.
        transform.Translate(Vector3.forward * Time.deltaTime * cellSpeed * cellSpeedModifier);

        // If it's spotted a target, decided whether to flee the target or pursue it.
        if (target != null)
        {
            // Start a countdown for a memory check of the target.
            StartCoroutine(ForgetTarget());
            if (IsCellStronger())
            {
                // Follow its target and when it closes the angle, charge.
                if (IsTargetInFront() && !IsHibernating()) cellState = EnergyState.high;
                FollowTarget();
            }
            else
            {
                // Avoid the target if its close and don't blindly charge the threat.
                if (!IsTargetInFront() && !IsHibernating()) cellState = EnergyState.high;
                FleeTarget();
            }
        }
        // Otherwise idly cruise by.
        else
        {
            cellState = EnergyState.low;
        }
    }

    private bool IsHibernating()
    {
        if (cellEnergy < cellEnergyMax / cellCriticalFraction) return true;
        return false;
    }

    // Assess if the cell is stronger than it's target.
    private bool IsCellStronger()
    {
        return cellStrength > target.GetComponent<SingleCell>().Strength;
    }

    // When the angle difference between the target and itself is small enough, is is considered in front.
    private bool IsTargetInFront()
    {
        Vector3 targetDirection = target.transform.position - transform.position;
        float targetAngle = Vector3.Angle(targetDirection, transform.forward);
        float angleDivider = IsCellStronger() ? 3.0f : 1.0f;
        if (targetAngle < closeEnoughAngle / angleDivider) return true;
        return false;
    }

    // Flee the existential threat.
    private void FleeTarget()
    {
        Quaternion rotTarget = Quaternion.LookRotation(
                new Vector3(-target.gameObject.transform.position.x, 0.0f, -target.gameObject.transform.position.z) -
                new Vector3(-transform.position.x, 0.0f, -transform.position.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, cellRotation * Time.deltaTime);
    }

    // Pursue the hapless victim.
    private void FollowTarget()
    {
        Quaternion rotTarget = Quaternion.LookRotation(
                new Vector3(target.gameObject.transform.position.x, 0.0f, target.gameObject.transform.position.z) -
                new Vector3(transform.position.x, 0.0f, transform.position.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, cellRotation * Time.deltaTime);
    }

    // If the cell spots another cell from an opposing team, track it.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cell"))
        {
            // If no target tracked, track this one.
            if (target == null) target = other.gameObject;
            // Threatening targets are the biggest worry.
            else if (other.gameObject.GetComponent<SingleCell>().Strength > cellStrength)
            {
                // If the new target is even more threatening than the previous one, track this instead.
                if (other.gameObject.GetComponent<SingleCell>().Strength > 
                    target.gameObject.GetComponent<SingleCell>().Strength)
                {
                    target = other.gameObject;
                }
            }
            // Weaker prey takes lower priority.
            else if (other.gameObject.GetComponent<SingleCell>().Strength < cellStrength)
            {
                // If the new target is even more tastier than the previous one, track this instead.
                if (other.gameObject.GetComponent<SingleCell>().Strength >
                    target.gameObject.GetComponent<SingleCell>().Strength)
                {
                    target = other.gameObject;
                }
            }
        }
        // Small food globules take lowest priority for targetting.
        else if (other.gameObject.CompareTag("Food") && target == null)
        {
            // If there is nothing more interesting, get the food.
            target = other.gameObject;
        }
    }

    private void OnDestroy()
    {
        GameManager.CurrentRivals--;
    }
}
