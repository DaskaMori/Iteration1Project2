using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public void Interact(GridMovement player)
    {
        if (player.keysCollected >= player.keysRequiredToWin)
        {
            Debug.Log("All keys collected! You win!");
        }
        else
        {
            Debug.Log($"You need {player.keysRequiredToWin - player.keysCollected} more keys.");
        }
    }
}