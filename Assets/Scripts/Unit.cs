using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
  
    GridPosition gridPosition;
    MoveAction moveAction;
    


    private void Awake()
    {
        //Debug.Log(targetPos); 
        // targetPos is initialized to 000 if unit is set to others it will update its position in update method.
        moveAction = GetComponent<MoveAction>();
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

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public GridPosition GetUnitGridPosition()
    {
        return gridPosition;
    }

   

}
