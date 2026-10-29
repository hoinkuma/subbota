using TMPro;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float reachDistance = 0.2f;

    [Header("Vision")]
    [SerializeField] private float viewDistance = 8f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask visionMask;
    [SerializeField] private EnemyVisionMesh visionMesh;

    private int currentPoint = 0;

    [SerializeField] private float detectionTime = 2f;
    [SerializeField] private GameObject alert;

    private float detectionTimer = 0f;

    void Start()
    {
        if (visionMesh != null)
        {
            visionMesh.SetParams(viewDistance, viewAngle);
        }
    }
    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.IsGameOver) return;

        Patrol();
        CheckVision();
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPoint];
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 5f * Time.deltaTime);
        }

        if (dir.magnitude < reachDistance)
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    void CheckVision()
    {
        if (player == null) return;

        Vector3 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance > viewDistance)
        {
            detectionTimer = 0f;
            SetAlert(false);
            return;
        }

        Vector3 flatDir = toPlayer;
        flatDir.y = 0f;

        float angle = Vector3.Angle(transform.forward, flatDir);

        if (angle > viewAngle)
        {
            detectionTimer = 0f;
            SetAlert(false);
            return;
        }

        Vector3 eye = transform.position + Vector3.up * 0.1f;
        Vector3 targetPoint = player.position + Vector3.up * 0.1f;

        Vector3 direction = targetPoint - eye;
        float rayDistance = direction.magnitude;

        if (Physics.Raycast(
            eye,
            direction.normalized,
            out RaycastHit hit,
            rayDistance,
            visionMask,
            QueryTriggerInteraction.Ignore))
        {
            if (!hit.transform.CompareTag("Player"))
            {
                detectionTimer = 0f;
                SetAlert(false);
                return;
            }
        }

        SetAlert(true);

        detectionTimer += Time.deltaTime;

        if (detectionTimer >= detectionTime)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.OnPlayerCaught();
            }
        }
    }
    void SetAlert(bool visible)
    {
        if (alert != null)
        {
            alert.SetActive(visible);
        }
    }
}