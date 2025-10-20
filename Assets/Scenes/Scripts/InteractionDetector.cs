using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;
    public GameObject interactionIcon;
    public GameObject pressToInteract;

    void Start()
    {
        interactionIcon.SetActive(false);
        pressToInteract.SetActive(false);
    }

    void Update()
    {
        // If current interactable has disappeared or been disabled, clear it
        if (interactableInRange == null || !((MonoBehaviour)interactableInRange).gameObject.activeInHierarchy)
        {
            ClearInteractable();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable;
            interactionIcon.SetActive(true);
            pressToInteract.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            ClearInteractable();
        }
    }

    private void ClearInteractable()
    {
        interactableInRange = null;
        interactionIcon.SetActive(false);
        pressToInteract.SetActive(false);
    }
}