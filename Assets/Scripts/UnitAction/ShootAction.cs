using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    [SerializeField] int max_Shoot_Dis = 7;

    float stateTimer;
    private Unit targetUnit;
    bool canShootBullet;
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit ShootingUnit;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }
    private State state;

    void Update()
    {

        if (!isActive) return;

        stateTimer -= Time.deltaTime;
        if (stateTimer < 0)
        {
            NextState();

        }
        switch (state)
        {
            case State.Aiming:
                float rotateSpeed = 20f;
                Vector3 unitDirection = (targetUnit.GetWorldPosition() - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, unitDirection, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    canShootBullet = false;
                    Shoot();
                }
                state = State.Cooloff;
                float coolOffStateTime = 2f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                break;
        }

    }


    void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = .2f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = 1f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:

                ActionComplete();
                break;

        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            ShootingUnit = unit
        });
        int damageAmount = 40;
        targetUnit.Damage(damageAmount);
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidGridPositionsList()
    {
        GridPosition unitGridPosition = unit.GetUnitGridPosition();
        return GetValidGridPositionsList(unitGridPosition);

    }


    public List<GridPosition> GetValidGridPositionsList(GridPosition unitGridPosition)
    {


        List<GridPosition> validGridPositions = new List<GridPosition>();
        for (int x = -max_Shoot_Dis; x <= max_Shoot_Dis; x++)
        {
            for (int z = -max_Shoot_Dis; z <= max_Shoot_Dis; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + unitGridPosition;

                //Test if outside the entire GridSystem
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                //Test if outside shooting range
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > max_Shoot_Dis) continue;

                //Pass if gridposition has no unit.
                if (!LevelGrid.Instance.HasUnitAtGridPosition(testGridPosition)) continue;

                //Now only gridposition has unit is left
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                //Check if on the same Team because both side use this script.
                if (targetUnit.IsEnemy() == this.unit.IsEnemy()) continue;

                validGridPositions.Add(testGridPosition);


            }
        }

        return validGridPositions;

    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = .2f;
        stateTimer = aimingStateTime;
        canShootBullet = true;
        ActionStart(onActionComplete);


    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }


    public int GetShootRange()
    {
        return max_Shoot_Dis;
    }

    public int GetTargetCountAtGridPosition(GridPosition gridPosition)
    {
        return GetValidGridPositionsList(gridPosition).Count;
    }

    public override EnemyActionValue GenerateEnemyActionValue(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyActionValue
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt(1 - targetUnit.GetNormalizedHealthValue()),
        };
    }
}



}
