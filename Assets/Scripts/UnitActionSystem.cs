using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
    // Start is called before the first frame update
    [SerializeField] Unit unitSelected;
    [SerializeField] LayerMask unitLayer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);

        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //select unit and publish selected event, but will not make unit move.
            if (TryHandleUnitSelection()) return; 
            // Set Target Position, then unit will execute Move()
            unitSelected.UpdateTargetPos(MousePos.GetMousePosition());
            
        }
    }

    bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayer))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                //Debug.Log(unit.name);
                return true;
            }
        }
        return false;

    }

    private void SetSelectedUnit(Unit unit)
    {
        unitSelected = unit;
        //OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);  
        //same as below.
        if (OnSelectedUnitChanged != null) //if there is subscriber
        {
            OnSelectedUnitChanged(this, EventArgs.Empty); //invoke event
        }
    }

    public Unit GetSelectedUnit()
    {
        return unitSelected;
    }
}
