using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class EnemyVisionMesh : MonoBehaviour
{
    [Header("Vision Mesh")]
    [SerializeField] private int rayCount = 60;
    private float heightOffset = 0.1f;

    [Header("Material")]
    [SerializeField] private Material visionMaterial;

    [Header("Obstacles")]
    [SerializeField] private LayerMask visionMask;

    private Mesh visionMesh;
    private float viewDistance;
    private float viewAngle;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public void SetParams(float distance, float angle)
    {
        viewDistance = distance;
        viewAngle = angle;
    }

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        visionMesh = new Mesh();
        visionMesh.name = "Enemy Vision Mesh";
        meshFilter.mesh = visionMesh;

        if (visionMaterial != null)
        {
            meshRenderer.material = visionMaterial;
        }

        meshRenderer.shadowCastingMode =
            UnityEngine.Rendering.ShadowCastingMode.Off;

        meshRenderer.receiveShadows = false;
    }

    void LateUpdate()
    {
        DrawVision();
    }

    void DrawVision()
    {
        if (visionMesh == null) return;

        Vector3 origin = transform.position
                       + Vector3.up * heightOffset;

        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = new Vector3(0f, heightOffset, 0f);

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = -viewAngle
                + (viewAngle * 2f * i / rayCount);

            Vector3 localDirection = Quaternion.Euler(0f, angle, 0f)
                * Vector3.forward;

            Vector3 worldDirection =
                transform.TransformDirection(localDirection);

            Vector3 endpoint = origin
                + worldDirection * viewDistance;

            if (Physics.Raycast(
                origin,
                worldDirection,
                out RaycastHit hit,
                viewDistance,
                visionMask,
                QueryTriggerInteraction.Ignore))
            {
                endpoint = hit.point;
            }

            vertices[i + 1] =
                transform.InverseTransformPoint(endpoint);
        }

        for (int i = 0; i < rayCount; i++)
        {
            int triangleIndex = i * 3;

            triangles[triangleIndex] = 0;
            triangles[triangleIndex + 1] = i + 1;
            triangles[triangleIndex + 2] = i + 2;
        }

        visionMesh.Clear();
        visionMesh.vertices = vertices;
        visionMesh.triangles = triangles;
        visionMesh.RecalculateNormals();
        visionMesh.RecalculateBounds();
    }
}