using JetBrains.Annotations;
using UnityEngine;

public class Podium : MonoBehaviour
{
    public GameObject BookPage;
    public void Interact()
    {
        BookPage.SetActive(true);

    }
}
