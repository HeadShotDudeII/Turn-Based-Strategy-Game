using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    float totalSpinAmount;
    int spinActionPoint = 2;

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        Spin();

    }

    public override void TakeAction(GridPosition gridPosition, Action onSpinActionComplete)
    {
        isActive = true;
        onActionComplete = onSpinActionComplete;
        //Debug.Log("totalSpinAmount " + totalSpinAmount);
        totalSpinAmount = 0f;
    }

    private void Spin()
    {
        float spinAmount = Time.deltaTime * 360f;
        Vector3 spinAmoutEngle = new Vector3(0, spinAmount, 0);
        transform.eulerAngles += spinAmoutEngle;
        totalSpinAmount += spinAmount;
        if (totalSpinAmount >= 360f)
        {
            isActive = false;
            onActionComplete();
        }


    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidGridPositionsList()
    {
        return new List<GridPosition> { UnitActionSystem.Instance.GetSelectedUnit().GetUnitGridPosition() };
    }

    public override int GetActionPoints()
    {
        return spinActionPoint;
    }

}
