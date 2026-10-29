using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.15f;
    private Vector3 velocity;

    private float rotateSpeed = 90f;

    void LateUpdate()
    {
        if (!target) return;
        Vector3 targetPos = target.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        if (Input.GetKey(KeyCode.Q))
            transform.RotateAround(target.position, Vector3.up, -rotateSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.E))
            transform.RotateAround(target.position, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}