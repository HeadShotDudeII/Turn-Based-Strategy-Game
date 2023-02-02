using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] Unit unitSelected;
    [SerializeField] LayerMask unitLayer;
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public BaseAction selectedBaseAction;

    public bool isBusy;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);

        }
        Instance = this;
    }


    private void Start()
    {
        SetSelectedUnit(unitSelected);
    }


    // Update is called once per frame
    void Update()
    {
        if (isBusy) return;
        if (Input.GetMouseButton(0))
        {
            //select unit and publish selected event, but will not make unit move.
            if (TryHandleUnitSelection()) return;
            // Set Target Position, then unit will execute Move()

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPositionFromWorldPos(MousePos.GetMousePosition());
            if (unitSelected.GetMoveAction().IsValidGridPosition(mouseGridPosition))
            {
                SetBusy();
                unitSelected.GetMoveAction().UpdateTargetPos(mouseGridPosition, ClearBusy);
            }

        }
        if (Input.GetMouseButton(1))
        {
            SetBusy();
            unitSelected.GetSpinAction().SpinStart(ClearBusy);
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

    public void SetSelectedUnit(Unit unit)
    {
        unitSelected = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        SetSelectedAction(unit.GetDefaultAction());


    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedBaseAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return unitSelected;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedBaseAction;
    }


    public void SetBusy()
    {
        isBusy = true;
    }

    public void ClearBusy()
    {
        isBusy = false;
    }
}
