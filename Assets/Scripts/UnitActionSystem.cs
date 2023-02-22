using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] Unit selectedUnit;
    [SerializeField] BaseAction selectedBaseAction;
    [SerializeField] LayerMask unitLayer;
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionTook;


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
        SetSelectedUnit(selectedUnit);
    }


    // Update is called once per frame
    void Update()
    {
        if (isBusy) return;

        if (!TurnSystem.Instance.IsPlayerTurn()) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TryHandleUnitSelection()) return;

        HandleBaseAction();
    }

    bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayer))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {

                    if (unit == selectedUnit) return false;

                    if (unit.IsEnemy()) return false;


                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;

    }

    void HandleBaseAction()
    {
        if (Input.GetMouseButton(0))
        {


            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPositionFromWorldPos(MousePos.GetMousePosition());

            if (!selectedBaseAction.IsValidGridPosition(mouseGridPosition)) return;

            if (!selectedUnit.TrySpendActionPoints(selectedBaseAction)) return;

            SetBusy();
            selectedBaseAction.TakeAction(mouseGridPosition, ClearBusy);
            OnActionTook?.Invoke(this, EventArgs.Empty);

        }


    }


    public void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);

        if (selectedBaseAction != null)
            selectedBaseAction = selectedUnit.GetSelectedBaseAction();
        else
            selectedBaseAction = selectedUnit.GetDefaultAction();

        SetSelectedAction(selectedBaseAction);





    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedBaseAction = baseAction;
        selectedUnit.SetSelectedBaseAction(baseAction);
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedBaseAction;
    }


    public void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);

    }

    public void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }
}
