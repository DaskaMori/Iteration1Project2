using UnityEngine;

public class ChestInteract : MonoBehaviour, IInteractable
{
    public void Interact(GridMovement player)
    {
        player.AddKey();
        Destroy(gameObject); 
    }
}