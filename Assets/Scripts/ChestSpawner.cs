using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject chestPrefab;
    public Transform[] waypoints;

    [Header("Options")]
    public bool spawnOnStart = true;

    void Start()
    {
        if (spawnOnStart)
            SpawnChests();
    }

    public void SpawnChests()
    {
        if (chestPrefab == null || waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("Spawner not set up properly.");
            return;
        }

        foreach (Transform waypoint in waypoints)
        {
            Instantiate(chestPrefab, waypoint.position, Quaternion.identity);
        }
    }
}