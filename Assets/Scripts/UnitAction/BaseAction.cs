using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected Action onActionComplete;
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    protected bool isActive; // true on, false off
    int defaultActionPoint = 1;
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

    public virtual int GetActionPoints()
    {
        return defaultActionPoint;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);

    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);

    }

    public Unit GetUnit()
    {
        return unit;
    }
}
