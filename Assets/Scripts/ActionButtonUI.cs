using TMPro;
using UnityEngine;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;
    void Start()
    {
        buttonText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetBaseAction(BaseAction action)
    {
        buttonText.text = action.GetActionName();
    }
}
