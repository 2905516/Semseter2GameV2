using UnityEngine;

public class moveCam : MonoBehaviour
{
    [SerializeField] public Transform cameraPosition; // Reference to the camera position transform

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position; // Update the position of this object to match the camera position    
    }
}

