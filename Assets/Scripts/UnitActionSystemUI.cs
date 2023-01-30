using System;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform unitActionButtonPrefab;
    [SerializeField] Transform unitActionButtonContainer;

    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;

        CreateUnitActionButtons();
    }

    void CreateUnitActionButtons()
    {
        foreach (Transform buttonUI in unitActionButtonContainer)
        {
            Destroy(buttonUI.gameObject);
        }

        Unit unitSelected = UnitActionSystem.Instance.GetSelectedUnit();


        foreach (BaseAction baseAction in unitSelected.GetBaseActions())
        {
            Transform actionButtonTransform = Instantiate(unitActionButtonPrefab, unitActionButtonContainer);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

        }
    }

    void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }
}
