using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonTextUI;
    [SerializeField] Button button;
    [SerializeField] GameObject buttonVisual;

    BaseAction baseAction;
    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        buttonTextUI.text = baseAction.GetActionName();
        button.onClick.AddListener(() => { UnitActionSystem.Instance.SetSelectedAction(baseAction); });

    }

    public void UpdateButtonUI(BaseAction selectedBaseAction)
    {
        buttonVisual.SetActive(baseAction == selectedBaseAction);
    }
}
