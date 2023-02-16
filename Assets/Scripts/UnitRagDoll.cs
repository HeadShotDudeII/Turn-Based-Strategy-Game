using UnityEngine;

public class UnitRagDoll : MonoBehaviour
{
    //ragDoll for dying interaction with gravity.
    [SerializeField] private Transform ragDollRootBone;
    [SerializeField] private float explosionForce = 1000f;
    [SerializeField] private float explosionRange = 10f;


    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransforms(originalRootBone, ragDollRootBone);
        ApplyExplosionToRagDoll(ragDollRootBone, explosionForce, transform.position, explosionRange);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagDoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToRagDoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
