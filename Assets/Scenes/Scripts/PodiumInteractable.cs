using UnityEngine;

public class PodiumInteractable : MonoBehaviour, IInteractable
{
    [Header("Podium Settings")]
    public GameObject imageToShow; // Assign the UI image GameObject
    private bool isActive = false;

    public void Interact()
    {
        isActive = !isActive;
        imageToShow.SetActive(isActive);
    }

    public bool CanInteract() => true;
}
