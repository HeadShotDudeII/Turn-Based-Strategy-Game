using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    float timer = .5f;
    State state;

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                Debug.Log("Enemy State inside switch " + state);
                break;
            case State.TakingTurn:

                timer -= Time.deltaTime;
                Debug.Log("Busy for " + timer);
                if (timer <= 0f)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                        Debug.Log("State inside TRYTakeAction " + state);

                    }
                    else
                    {
                        // No more enemies have actions they can take, end enemy turn
                        TurnSystem.Instance.NextTurn();
                    }
                }

                break;
            case State.Busy:
                Debug.Log("Enemy State inside switch " + state);
                break;
        }




    }


    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
            Debug.Log("state is " + state);

        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
        Debug.Log("Set State Taking Turn " + state);
    }



    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        Debug.Log("Take Enemy AI Action");
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPosition actionGridPosition = enemyUnit.GetGridPosition();

        if (!spinAction.IsValidGridPosition(actionGridPosition))
        {
            Debug.Log("IsValidGridPosition");
            return false;
        }

        if (!enemyUnit.TryTakeActionAndSpendActionPoints(spinAction))
        {
            Debug.Log("TryTakeActionAndSpendActionPoints");
            return false;
        }


        spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
        Debug.Log("Spin Action! Enemy State is " + state);
        return true;
    }





}
