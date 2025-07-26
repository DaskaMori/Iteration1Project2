using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class StaticFOV : MonoBehaviour
{
    [Header("Vision Settings")]
    [SerializeField] private float coneAngle = 90f;
    [SerializeField] private float coneDistance = 0.6f;
    [SerializeField] private float circleDistance = 0.3f;
    public int rayCount = 30;
    public LayerMask obstacleMask;

    private Mesh mesh;
    private PolygonCollider2D polyCollider;
    private GridMovement gridMovement;

    private Vector2 direction = Vector2.down;
    private bool isCircleMode = false;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        polyCollider = GetComponent<PolygonCollider2D>();
        gridMovement = transform.parent.GetComponent<GridMovement>();
    }

    void LateUpdate()
    {
        // Toggle vision mode
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isCircleMode = !isCircleMode;
        }

        direction = gridMovement.LastDirection;
        UpdateFOVMesh();
    }

    void UpdateFOVMesh()
    {
        Vector3 origin = transform.parent.position;
        transform.position = origin;

        float fovAngle = isCircleMode ? 360f : coneAngle;
        float viewDistance = isCircleMode ? circleDistance : coneDistance;

        float angleOffset = GetAngleFromVector(direction);
        float startAngle = isCircleMode ? 0f : angleOffset - fovAngle / 2f;
        float angleIncrease = fovAngle / rayCount;

        List<Vector3> vertices = new List<Vector3> { Vector3.zero };
        List<int> triangles = new List<int>();

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = startAngle + i * angleIncrease;
            Vector3 dir = GetVectorFromAngle(angle).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewDistance, obstacleMask);

            if (hit.collider == null)
                vertices.Add(dir * viewDistance);
            else
                vertices.Add((Vector3)hit.point - transform.position);
        }

        for (int i = 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        if (polyCollider != null)
        {
            Vector2[] colliderPoints = new Vector2[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                colliderPoints[i] = new Vector2(vertices[i].x, vertices[i].y);
            }
            polyCollider.SetPath(0, colliderPoints);
        }
    }

    Vector3 GetVectorFromAngle(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    float GetAngleFromVector(Vector2 dir)
    {
        if (dir == Vector2.up) return 90f;
        if (dir == Vector2.down) return 270f;
        if (dir == Vector2.left) return 180f;
        if (dir == Vector2.right) return 0f;
        return 0f;
    }
    
    public bool IsInCircleMode()
    {
        return isCircleMode;
    }
}
