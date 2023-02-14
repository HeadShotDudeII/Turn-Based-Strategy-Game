using System;

using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] Animator unitAnimator;
    float smallDis = 0.1f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] int max_Move_Dis = 1;

    Vector3 targetPos;
    GridPosition unitGridPosition;
    int moveActionPoint = 1;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;


    protected override void Awake()
    {
        base.Awake();
        targetPos = transform.position;

    }

    void Update()
    {
        if (!isActive) return;
        Move();
    }

    private void Move()
    {

        Vector3 unitDirection = (targetPos - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPos) > smallDis)
        {
            //Direction and normalized distance
            transform.position += unitDirection * moveSpeed * Time.deltaTime;
            //notice this lerp is not linear since transform.forward is chaning if linear is needed it need to be stored every change in direction.
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            isActive = false;

            OnStopMoving?.Invoke(this, EventArgs.Empty);

            onActionComplete();

        }
        transform.forward = Vector3.Lerp(transform.forward, unitDirection, Time.deltaTime * rotateSpeed);

    }

    public override void TakeAction(GridPosition gridPosition, Action onMoveActionComplete)
    {
        isActive = true;
        onActionComplete = onMoveActionComplete;
        //if targetPos is different from transform.position will trigger the Move();
        this.targetPos = LevelGrid.Instance.GetWorldPositionFromGridPos(gridPosition);
        OnStartMoving?.Invoke(this, EventArgs.Empty);



    }

    public override List<GridPosition> GetValidGridPositionsList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetUnitGridPosition();
        for (int x = -max_Move_Dis; x <= max_Move_Dis; x++)
        {
            for (int z = -max_Move_Dis; z <= max_Move_Dis; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + unitGridPosition;
                if (testGridPosition == unitGridPosition) continue;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (LevelGrid.Instance.HasUnitAtGridPosition(testGridPosition)) continue;
                validGridPositions.Add(testGridPosition);

                //Debug.Log("x is "+x +"z is "+z+"Valid grid is " + testGridPosition.ToString());

            }
        }

        return validGridPositions;
    }




    public override string GetActionName()
    {
        return "Move";
    }

    public override int GetActionPoints()
    {
        return moveActionPoint;
    }


}
