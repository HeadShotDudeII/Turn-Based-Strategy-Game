using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
                    SetSelectedUnit(unit);
                    //Debug.Log(unit.name);
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

            if (selectedBaseAction.IsValidGridPosition(mouseGridPosition))
            {
                SetBusy();
                selectedBaseAction.TakeAction(mouseGridPosition, ClearBusy);
            }

        }


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
