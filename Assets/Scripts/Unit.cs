using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] Animator unitAnimator;
    Vector3 targetPos;
    GridPosition gridPosition;
    float smallDis = 0.1f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float rotateSpeed = 10f;


    private void Awake()
    {
        //Debug.Log(targetPos); 
        // targetPos is initialized to 000 if unit is set to others it will update its position in update method.
        this.targetPos = transform.position;
        //so the unit will stay where it was set instead of going to 000
    }


    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPositionFromWorldPos(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
               
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        UpdateGridPosition();

    }

    private void UpdateGridPosition()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPositionFromWorldPos(transform.position);
        //if(currentGridPosition.x==gridPosition.x && currentGridPosition.z == gridPosition.z) {        }
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
            // Debug.Log("grid position changed");
        }
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

    public void UpdateTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
      

    }

}
