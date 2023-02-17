using System;
using UnityEngine;

public class UnitRagDollSpawner : MonoBehaviour
{

    [SerializeField] Transform ragDollSpawnerPrefab;
    [SerializeField] private Transform originalRootBone;

    private HealthSystem healthSystem;
    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragDollTransform = Instantiate(ragDollSpawnerPrefab, transform.position, transform.rotation);
        UnitRagDoll unitRagDoll = ragDollTransform.GetComponent<UnitRagDoll>();
        unitRagDoll.Setup(originalRootBone);

    }


}
