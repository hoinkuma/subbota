
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody rb;
    private Vector3 movement;

    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float slowSpeed = 1f;

    private float currentSpeed;
    private bool isSlowed = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = normalSpeed;
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        movement = new Vector3(x, 0f, z).normalized;

    }

    private void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;

        velocity.x = movement.x * currentSpeed;
        velocity.z = movement.z * currentSpeed;

        rb.linearVelocity = velocity;
    }
    public void SetSlowed(bool slowed)
    {
        isSlowed = slowed;
        currentSpeed = slowed ? slowSpeed : normalSpeed;
    }
}