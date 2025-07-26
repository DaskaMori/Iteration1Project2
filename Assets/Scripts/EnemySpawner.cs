using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyPrefab;
    public Transform[] waypoints;

    [Header("Options")]
    public bool spawnOnStart = true;

    void Start()
    {
        if (spawnOnStart)
            SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if (enemyPrefab == null || waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("Spawner not set up properly.");
            return;
        }

        foreach (Transform waypoint in waypoints)
        {
            Instantiate(enemyPrefab, waypoint.position, Quaternion.identity);
        }
    }
}