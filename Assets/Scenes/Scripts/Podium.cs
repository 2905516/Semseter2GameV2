using JetBrains.Annotations;
using UnityEngine;

public class Podium : MonoBehaviour, IInteractable
{
    public GameObject BookPage;
    private bool isInteracted = false;
    public bool CanInteract()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        if (isInteracted) 
        {
            BookPage.SetActive(true);
            isInteracted = true;    
        }
        else
        {
            BookPage.SetActive(false);
            isInteracted = false;
        }


    }
}
