using UnityEngine;

public class ChestVisibility : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool isRevealed = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isRevealed) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("VisionCone")) return;

        StaticFOV fov = other.GetComponent<StaticFOV>();
        if (fov == null || !fov.IsInCircleMode()) return;

        // Check line of sight
        Vector2 origin = other.transform.position;
        Vector2 toChest = (Vector2)transform.position - origin;
        float dist = toChest.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            toChest.normalized,
            dist,
            LayerMask.GetMask("Obstacles")
        );

        if (hit.collider == null)
        {
            sr.enabled = true;
            isRevealed = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isRevealed) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("VisionCone")) return;

        StaticFOV fov = other.GetComponent<StaticFOV>();
        if (fov == null || !fov.IsInCircleMode()) return;

        Vector2 origin = other.transform.position;
        Vector2 toChest = (Vector2)transform.position - origin;
        float dist = toChest.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            toChest.normalized,
            dist,
            LayerMask.GetMask("Obstacles")
        );

        if (hit.collider == null)
        {
            sr.enabled = true;
            isRevealed = true;
        }
    }
}