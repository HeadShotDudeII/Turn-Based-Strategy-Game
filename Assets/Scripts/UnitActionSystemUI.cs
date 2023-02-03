using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform buttonContainer;
    [SerializeField] Transform buttonPrefab;

    List<ActionButtonUI> buttonList;
    BaseAction selectedBaseAction;
    Unit selectedUnit;

    private void Awake()
    {
        buttonList = new List<ActionButtonUI>();
    }
    private void Start()
    {

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;


        CreateSelectedUnitButtons();
        UpdateButtonsVisual();


    }

    private void CreateSelectedUnitButtons()
    {
        selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (Transform buttonHolder in buttonContainer)
        {
            Destroy(buttonHolder.gameObject);
        }

        buttonList.Clear();

        BaseAction[] baseActions = selectedUnit.GetBaseActions();

        foreach (BaseAction baseAction in baseActions)
        {
            Transform buttonUI = Instantiate(buttonPrefab, buttonContainer);
            ActionButtonUI actionButton = buttonUI.GetComponent<ActionButtonUI>();
            actionButton.SetBaseAction(baseAction);
            buttonList.Add(actionButton);
        }


    }

    private void UpdateButtonsVisual()
    {
        selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        foreach (ActionButtonUI actionButton in buttonList)
        {
            actionButton.UpdateButtonUI(selectedBaseAction);
        }
    }

    void UnitActionSystem_OnSelectedUnitChanged(object Sender, EventArgs e)
    {
        CreateSelectedUnitButtons();
    }

    void UnitActionSystem_OnSelectedActionChanged(object Sender, EventArgs e)
    {
        UpdateButtonsVisual();
    }
}
