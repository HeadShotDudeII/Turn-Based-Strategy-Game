using System;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    private int actionPoints = 4;
    [SerializeField] bool isEnemy;


    GridPosition gridPosition;
    private HealthSystem healthSystem;
    MoveAction moveAction;
    SpinAction spinAction;
    ShootAction shootAction;
    BaseAction[] baseActionArray;
    BaseAction selectedBaseAction;


    [SerializeField] public const int MAX_DEFAULT_ACTION_POINTS = 4;






    private void Awake()
    {
        //Debug.Log(targetPos); 
        // targetPos is initialized to 000 if unit is set to others it will update its position in update method.
        healthSystem = GetComponent<HealthSystem>();
        spinAction = GetComponent<SpinAction>();
        shootAction = GetComponent<ShootAction>();
        baseActionArray = GetBaseActions();
        selectedBaseAction = GetDefaultAction();
        //so the unit will stay where it was set instead of going to 000
    }


    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPositionFromWorldPos(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

        healthSystem.OnDead += HealthSystem_OnDead;

    }



    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
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
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);

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

    public ShootAction GetShootAction()
    {
        return shootAction;
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

    public bool CanSpendActionPoints(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    public bool TrySpendActionPoints(BaseAction baseAction)
    {
        if (actionPoints < baseAction.GetActionPointsCost())
            return false;
        else
        {
            actionPoints -= baseAction.GetActionPointsCost();
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

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public float GetNormalizedHealthValue()
    {
        return healthSystem.GetHealthPercentage();
    }


}
