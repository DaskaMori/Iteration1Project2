using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMovement : MonoBehaviour
{
    private bool isMoving = false;
    [SerializeField] public bool isAlive = true;
    private SpriteRenderer spriteRenderer;
    public Vector3 LastDirection { get; private set; } = Vector3.down;
    public float tileSize = 0.16f;
    public float moveSpeed = 2f;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootingRange = 0.6f;
    [SerializeField] private Transform shooterTransform;

    public Tilemap Walls;

    public int keysCollected = 0;
    public int keysRequiredToWin = 15;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UIManager.Instance.UpdateKeyUI(keysCollected);

    }

    void Update()
    {
        if (!isAlive || isMoving) return;

        Vector3 dir = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W)) dir = Vector3.up;
        if (Input.GetKeyDown(KeyCode.S)) dir = Vector3.down;
        if (Input.GetKeyDown(KeyCode.A)) dir = Vector3.left;
        if (Input.GetKeyDown(KeyCode.D)) dir = Vector3.right;

        if (dir != Vector3.zero && !IsBlocked(dir))
        {
            LastDirection = dir;
            spriteRenderer.flipX = dir.x < 0;
            StartCoroutine(MoveOneTile(dir));
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Transform target = FindClosestEnemy();
            if (target != null)
            {
                Shoot(target);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private bool IsBlocked(Vector3 direction)
    {
        Vector3 worldDest = transform.position + direction * tileSize;
        Vector3Int cellPos = Walls.WorldToCell(worldDest);
        return Walls.HasTile(cellPos);
    }

    private IEnumerator MoveOneTile(Vector3 direction)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction * tileSize;
        float duration = tileSize / moveSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
    }
    
    private bool IsInVisionCone(Transform target)
    {
        Vector3 toTarget = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(LastDirection, toTarget);

        if (angle > 45f) return false; 

        RaycastHit2D hit = Physics2D.Raycast(transform.position, toTarget, shootingRange, LayerMask.GetMask("Obstacles"));

        if (hit.collider != null)
        {
            Debug.Log($"Ray hit: {hit.collider.name}");

            Debug.Log($"Blocked by: {hit.collider.gameObject.name}");
            return true;
        }

        return true;
    }


    private void Shoot(Transform target)
    {
        Vector3 spawnPos = shooterTransform != null ? shooterTransform.position : transform.position;

        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.SetTarget(target);
        projectile.SetOwner(gameObject);
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float minDist = shootingRange;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist && IsInVisionCone(enemy.transform))
            {
                minDist = dist;
                closest = enemy.transform;
            }
        }

        return closest;
    }
    
    private void TryInteract()
    {
        Vector2 origin = transform.position;
        Vector2 direction = LastDirection;
        float distance = 0.2f;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask("Interactable"));
    
        if (hit.collider != null)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this);
            }
        }
    }

    
    public void AddKey()
    {
        keysCollected++;
        UIManager.Instance.UpdateKeyUI(keysCollected);
    }
}
