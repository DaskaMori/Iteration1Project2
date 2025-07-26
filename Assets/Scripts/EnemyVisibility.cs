using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
    private SpriteRenderer sr;
    public bool isDetected;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;         // Hidden at start
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("VisionCone"))
            return;

        // Raycast (LOS) from VisionCone origin to this enemy
        Vector2 origin = other.transform.position;
        Vector2 toEnemy = (Vector2)transform.position - origin;
        float dist = toEnemy.magnitude;
        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            toEnemy.normalized,
            dist,
            LayerMask.GetMask("Obstacles")
        );

        if (hit.collider == null)
        {
            // First time spotted?
            if (!isDetected)
            {
                isDetected = true;
                GameManager.Instance.OnEnemyDetected();
            }
            sr.enabled = true;
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("VisionCone")) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("VisionCone"))
            return;

        // Raycast (LOS) from VisionCone origin to this enemy
        Vector2 origin = other.transform.position;
        Vector2 toEnemy = (Vector2)transform.position - origin;
        float dist = toEnemy.magnitude;
        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            toEnemy.normalized,
            dist,
            LayerMask.GetMask("Obstacles")
        );

        if (hit.collider == null)
        {
            // First time spotted?
            if (!isDetected)
            {
                isDetected = true;
                GameManager.Instance.OnEnemyDetected();
            }
            sr.enabled = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("VisionCone"))
            return;

        if (sr.enabled && GameManager.Instance.markersEnabled)
        {
            Instantiate(
                GameManager.Instance.lastSeenPrefab,
                transform.position,
                Quaternion.identity
            );
        }
        sr.enabled = false;
        isDetected = false;
    }
}