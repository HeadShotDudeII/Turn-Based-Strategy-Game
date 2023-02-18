using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] MeshRenderer gridMesh;

    public void ShowVisual(Material material)
    {
        gridMesh.enabled = true;
        gridMesh.material = material;

    }

    public void HideVisual()
    {
        gridMesh.enabled = false;

    }

}
