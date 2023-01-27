using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected Action onActionComplete;
    protected bool isActive; // true on, false off
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
