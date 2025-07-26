using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Marker Settings")]
    public bool markersEnabled = true;
    public GameObject lastSeenPrefab;

    private int moves;
    private int detectedCount;
    private int totalEnemies;
    private float startTime;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    void Start()
    {
        startTime = Time.time;
    }

    public void RegisterMove()
    {
        moves++;
    }

    public void OnEnemyDetected()
    {
        detectedCount++;
        Debug.Log($"Spotted {detectedCount}/{totalEnemies} enemies");

        if (detectedCount >= totalEnemies)
        {
            float duration = Time.time - startTime;
            Debug.Log(
                $"Mode: {(markersEnabled ? "With" : "Without")} Markers | " +
                $"Time: {duration:F2}s | Moves: {moves}"
            );
        }
    }
}