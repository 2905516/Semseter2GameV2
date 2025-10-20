using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    [Tooltip("The actual door object that should appear/disappear.")]
    public GameObject doorObject;

    [Header("Audio Settings")]
    [Tooltip("Sound played when door opens or closes.")]


    private bool isOpen = true;

    void Start()
    {
        if (doorObject == null)
            doorObject = this.gameObject; // fallback, controls this object

        // Ensure initial state is correct
        doorObject.SetActive(isOpen);
    }

    public void Interact()
    {
        // Toggle door on/off
        isOpen = !isOpen;
        doorObject.SetActive(isOpen);

        // Play sound
        SoundEffectManager.Play("DoorOpen");

    }

    public bool CanInteract() => true;
}