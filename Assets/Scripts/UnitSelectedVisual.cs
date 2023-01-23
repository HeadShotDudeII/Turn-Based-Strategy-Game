using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    //SHould be named UnitVisual it may or maynot be selected
    private MeshRenderer meshRenderer;
    [SerializeField] Unit unit;


    // Start is called before the first frame update
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged; 
        // subscribe to the event during start and event
        // the event will be triggered in update in UnitActionSystem
        UpdateVisual(); // for initialize to check if its selected before game start.

    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        UpdateVisual();

    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
            meshRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
