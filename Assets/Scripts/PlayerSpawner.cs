using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player Setup")]
    public GameObject playerPrefab;
    public Transform spawnPoint;

    private GameObject currentPlayer;

    void Start()
    {
        FindOrSpawnPlayer();
    }

    public void FindOrSpawnPlayer()
    {
        currentPlayer = GameObject.FindGameObjectWithTag("Player");

        if (currentPlayer != null)
        {
            GridMovement movement = currentPlayer.GetComponent<GridMovement>();
            if (movement != null && movement.isAlive)
            {
                currentPlayer.transform.position = spawnPoint.position;
                return;
            }
            else
            {
                Destroy(currentPlayer);
            }
        }

        currentPlayer = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
    }
}