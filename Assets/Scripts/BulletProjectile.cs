using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    Vector3 targetPos;
    [SerializeField] private TrailRenderer trailRenderer;

    public void SetBullet(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }

    private void Update()
    {
        Vector3 moveDir = (targetPos - transform.position).normalized;
        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPos);
        float moveSpeed = 200f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        float distanceAfterMoving = Vector3.Distance(transform.position, targetPos);
        if (distanceBeforeMoving < distanceAfterMoving)
        {
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
        }


    }


}
