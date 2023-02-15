using System;
using UnityEngine;

public class Unit : MonoBehaviour
{


    GridPosition gridPosition;
    private HealthSystem healthSystem;
    MoveAction moveAction;
    SpinAction spinAction;
    BaseAction[] baseActionArray;
    [SerializeField] int actionPoints = 3;
    [SerializeField] bool isEnemy;
    public const int MAX_DEFAULT_ACTION_POINTS = 3;
    BaseAction selectedBaseAction;
    public static event EventHandler OnAnyActionPointsChanged;





    private void Awake()
    {
        //Debug.Log(targetPos); 
        // targetPos is initialized to 000 if unit is set to others it will update its position in update method.
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetBaseActions();
        selectedBaseAction = GetDefaultAction();
        //so the unit will stay where it was set instead of going to 000
    }


    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPositionFromWorldPos(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.onDead += HealthSystem_OnDead;

    }


    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
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

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetUnitGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActions()
    {
        return GetComponents<BaseAction>();
    }

    public BaseAction GetDefaultAction()
    {
        return baseActionArray[0];
    }

    public bool TryTakeActionAndSpendActionPoints(BaseAction baseAction)
    {
        if (actionPoints < baseAction.GetActionPoints())
            return false;
        else
        {
            actionPoints -= baseAction.GetActionPoints();
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    public BaseAction GetSelectedBaseAction()
    {
        return selectedBaseAction;
    }

    public void SetSelectedBaseAction(BaseAction selectedBaseAction)
    {
        this.selectedBaseAction = selectedBaseAction;
    }

    public void TurnSystem_OnTurnChanged(object sendern, EventArgs e)
    {
        ResetActionPoints();
    }

    public void ResetActionPoints()
    {   // when isEnemy is true, and it's Enemy's turn dont update 
        // when isEnemy is false ?? 
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn())
            || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = MAX_DEFAULT_ACTION_POINTS;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
            // this is a static event will cause all instance to 
            // fire a event and all instance will execute what ever
            // code that listen to this event.
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }
}
