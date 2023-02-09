using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform buttonContainer;
    [SerializeField] Transform buttonPrefab;
    [SerializeField] TextMeshProUGUI actionPointsText;

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
        UnitActionSystem.Instance.OnActionTook += UnitActionSystem_OnActionTook;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyAcitonPointsChanged;

        CreateSelectedUnitButtons();
        UpdateButtonsVisual();
        updateActionPoints();


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
        CreateSelectedUnitButtons(); //will also trigger action changed in unitacitonsystem
        updateActionPoints();
    }

    void UnitActionSystem_OnSelectedActionChanged(object Sender, EventArgs e)
    {
        UpdateButtonsVisual();
    }

    private void UnitActionSystem_OnActionTook(object sender, EventArgs e)
    {
        updateActionPoints();
    }

    private void Unit_OnAnyAcitonPointsChanged(object sender, EventArgs e)
    {
        updateActionPoints();
    }



    void updateActionPoints()
    {
        actionPointsText.text = "Action Point is " + UnitActionSystem.Instance.GetSelectedUnit().GetActionPoints();
    }

}
