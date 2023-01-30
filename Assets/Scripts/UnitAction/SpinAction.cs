using System;
using UnityEngine;

public class SpinAction : BaseAction
{
    float totalSpinAmount;

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        Spin();

    }

    public void SpinStart(Action onSpinActionComplete)
    {
        isActive = true;
        onActionComplete = onSpinActionComplete;
        //Debug.Log("totalSpinAmount " + totalSpinAmount);
        totalSpinAmount = 0f;
    }

    private void Spin()
    {
        float spinAmount = Time.deltaTime * 360f;
        Vector3 spinAmoutEngle = new Vector3(0, spinAmount, 0);
        transform.eulerAngles += spinAmoutEngle;
        totalSpinAmount += spinAmount;
        if (totalSpinAmount >= 360f)
        {
            isActive = false;
            onActionComplete();
        }


    }

    public override string GetActionName()
    {
        return "Spin";
    }
}
