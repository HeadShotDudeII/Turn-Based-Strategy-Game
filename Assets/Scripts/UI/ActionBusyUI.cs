using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
        HideBusy();
    }

    void UnitActionSystem_OnBusyChanged(object sender, bool isbusy)
    {
        if (isbusy)
        {
            ShowBusy();
        }
        else
        {
            HideBusy();
        }
    }

    void ShowBusy()
    {
        gameObject.SetActive(true);
    }

    void HideBusy()
    {
        gameObject.SetActive(false);
    }



}
