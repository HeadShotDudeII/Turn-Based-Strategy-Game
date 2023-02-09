using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    float timer = 2f;

    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {

            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                TurnSystem.Instance.NextTurn();
            }

        }


    }


    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        timer = 2f;
    }



}
