using System;
using System.Collections.Generic;
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

    public abstract List<GridPosition> GetValidGridPositionsList();


    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return GetValidGridPositionsList().Contains(gridPosition);
    }

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public abstract string GetActionName();
}
