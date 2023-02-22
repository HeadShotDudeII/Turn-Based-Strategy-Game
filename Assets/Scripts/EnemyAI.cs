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
        EnemyActionValue bestEnemyActionValue = null;
        BaseAction bestBaseAction = null;
        foreach (BaseAction baseAction in enemyUnit.GetBaseActions())
        {
            if (!enemyUnit.CanSpendActionPoints(baseAction)) continue;
            if(bestEnemyActionValue == null)
            {
                bestEnemyActionValue = baseAction.GetBestEnemyActionValue();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyActionValue testEnemyActionValue = baseAction.GetBestEnemyActionValue();
                if(testEnemyActionValue!=null && testEnemyActionValue.actionValue > bestEnemyActionValue.actionValue)
                {
                    bestEnemyActionValue = testEnemyActionValue;
                    bestBaseAction = baseAction;
                }

            }
        }



        if (bestEnemyActionValue != null && enemyUnit.TrySpendActionPoints(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyActionValue.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else {



            return false;

        }


    }





}
