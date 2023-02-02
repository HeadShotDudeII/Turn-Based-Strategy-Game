using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform buttonContainer;
    [SerializeField] Transform buttonPrefab;


    Unit selectedUnit;
    BaseAction selectedAction;
    ActionButtonUI actionButtonUI;
    List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }


    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;


        CreateActionButtons();
        UpdateButtonsVisual();


    }

    private void CreateActionButtons()
    {
        selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();


        foreach (Transform buttonSlot in buttonContainer)
        {
            Destroy(buttonSlot.gameObject);
        }

        actionButtonUIList.Clear();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActions())
        {
            Transform buttonUI = Instantiate(buttonPrefab, buttonContainer);
            actionButtonUI = buttonUI.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
            actionButtonUIList.Add(actionButtonUI);
        }



    }


    private void UpdateButtonsVisual()
    {
        selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        foreach (ActionButtonUI actionButton in actionButtonUIList)
        {
            actionButton.UpdateButtonVisual(selectedAction);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateActionButtons();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateButtonsVisual();
    }

}
