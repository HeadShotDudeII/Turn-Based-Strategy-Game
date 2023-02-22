using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    float totalSpinAmount;
    [SerializeField] int spinActionPoint = 1;

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        Spin();

    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        //Debug.Log("totalSpinAmount " + totalSpinAmount);
        totalSpinAmount = 0f;
        ActionStart(onActionComplete);
    }

    private void Spin()
    {
        float spinAmount = Time.deltaTime * 360f;
        Vector3 spinAmoutEngle = new Vector3(0, spinAmount, 0);
        transform.eulerAngles += spinAmoutEngle;
        totalSpinAmount += spinAmount;
        Debug.Log("Spinning " + totalSpinAmount);
        if (totalSpinAmount >= 360f)
        {
            ActionComplete();
        }


    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidGridPositionsList()
    {
        return new List<GridPosition> { unit.GetUnitGridPosition() };
    }

    public override int GetActionPointsCost()
    {
        return spinActionPoint;
    }

    public override EnemyActionValue GenerateEnemyActionValue(GridPosition gridPosition)
    {
        return new EnemyActionValue
        {
            gridPosition = gridPosition,
            actionValue = 5,
        };

    }

}
