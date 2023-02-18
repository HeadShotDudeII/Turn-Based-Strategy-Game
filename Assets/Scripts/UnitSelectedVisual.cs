using System;
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
            //Debug.Log("the unit giving error is " + unit.ToString());
            meshRenderer.enabled = true;
        }
        else
        {
            //Debug.Log("the unit giving error is " + unit.ToString());

            meshRenderer.enabled = false;
        }
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;

    }


}
