using UnityEngine;

public class LookAtCamer : MonoBehaviour
{
    private Transform mainCamera;
    [SerializeField] private bool invert;

    private void Start()
    {
        mainCamera = Camera.main.transform;

    }

    private void LateUpdate()
    {
        if (invert)
        {
            Vector3 dirToCamera = (mainCamera.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCamera * -1);
            //transform.LookAt(dirToCamera);

        }
        else
        {
            transform.LookAt(mainCamera);
        }
    }
}
