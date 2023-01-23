using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    private GridObject gridObject;
    [SerializeField] private TextMeshPro textUI;

    public void ConnectGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
        //DisplayPos(gridObject);
    }

    public void DisplayPos(GridObject gridObject)
    {        
            textUI.text = gridObject.ToString();        
    }
}
