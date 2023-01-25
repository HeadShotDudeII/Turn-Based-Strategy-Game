using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] MeshRenderer gridMesh;

    public void ShowVisual()
    {
        gridMesh.enabled = true;

    }

    public void HideVisual()
    {
        gridMesh.enabled = false;

    }

}
