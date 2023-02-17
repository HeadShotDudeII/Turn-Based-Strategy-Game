using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxhealth = 100;
    private int health;


    public event EventHandler OnDead;
    public event EventHandler OnDamage;


    private void Awake()
    {
        health = maxhealth;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        OnDamage?.Invoke(this, EventArgs.Empty);
        if (health < 0)
        {
            health = 0;
        }

        if (health == 0)
        {
            Die();
        }
    }

    private void Die()
    {


        OnDead?.Invoke(this, EventArgs.Empty);


    }

    public float GetHealthPercentage()
    {
        return health / (float)maxhealth;
    }
}
