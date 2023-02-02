using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    BaseAction baseAction;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] GameObject selectedVisual;
    [SerializeField] Button button;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        buttonText.text = baseAction.GetActionName();
        button.onClick.AddListener(() => { UnitActionSystem.Instance.SetSelectedAction(baseAction); });

    }

    public void UpdateButtonVisual(BaseAction selectedAction)
    {
        selectedVisual.SetActive(baseAction == selectedAction);
    }
}
