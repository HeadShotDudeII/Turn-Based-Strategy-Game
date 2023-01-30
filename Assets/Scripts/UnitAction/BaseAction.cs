using System;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected Action onActionComplete;
    protected bool isActive; // true on, false off
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();

    }

    public abstract string GetActionName();
}
