using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.15f;
    private Vector3 velocity;

    void LateUpdate()
    {
        if (!target) return;
        Vector3 targetPos = target.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}