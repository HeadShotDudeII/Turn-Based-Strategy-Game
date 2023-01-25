using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] Animator unitAnimator;
    float smallDis = 0.1f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] int max_Move_Dis = 1;
    Unit unit;
    Vector3 targetPos;
    GridPosition unitGridPosition;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        targetPos = transform.position;
        
    }
  
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, targetPos) > smallDis)
        {
            Vector3 unitDirection = (targetPos - transform.position).normalized;
            //Direction and normalized distance
            transform.position += unitDirection * moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, unitDirection, Time.deltaTime * rotateSpeed);
            //notice this lerp is not linear since transform.forward is chaning if linear is needed it need to be stored every change in direction.
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);

        }
    }

    public void UpdateTargetPos(GridPosition gridPosition)
    {

        this.targetPos = LevelGrid.Instance.GetWorldPositionFromGridPos(gridPosition);


    }

    public List<GridPosition> GetValidGridPositionsList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetUnitGridPosition();
        for (int x = -max_Move_Dis; x <= max_Move_Dis; x++)
        {
            for(int z = -max_Move_Dis; z<= max_Move_Dis; z++)
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


    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return GetValidGridPositionsList().Contains(gridPosition);
    }



}
