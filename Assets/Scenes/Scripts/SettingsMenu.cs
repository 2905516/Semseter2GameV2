using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider fovSlider;
    public Slider sensitivitySlider;
    public Camera playerCamera;
    private FPController fpController;

    private void Start()
    {
        fpController = FindObjectOfType<FPController>();

        if (playerCamera != null)
            fovSlider.value = playerCamera.fieldOfView;

        if (fpController != null)
            sensitivitySlider.value = fpController.lookSensitivity;

        // Add listeners
        fovSlider.onValueChanged.AddListener(UpdateFOV);
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
    }

    private void UpdateFOV(float value)
    {
        if (playerCamera != null)
            playerCamera.fieldOfView = value;
    }

    private void UpdateSensitivity(float value)
    {
        if (fpController != null)
            fpController.lookSensitivity = value;
    }
}
