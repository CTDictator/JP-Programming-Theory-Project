using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleCell : MonoBehaviour
{
    [SerializeField] protected float cellSpeed = 20.0f;
    [SerializeField] protected float cellRotation;

    protected abstract void MoveCell();
}
