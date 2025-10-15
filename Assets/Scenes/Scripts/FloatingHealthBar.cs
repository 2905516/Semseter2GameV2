using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void UpdateSnakeHealthBar(float currentValue, float maxValue)
    {
        camera = Camera.main;
        slider.value = currentValue / maxValue;


    }

    public void UpdateGolemHealthBar(float currentValue, float maxValue)
    {
        camera = Camera.main;
        slider.value = currentValue / maxValue;


    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = camera.transform.rotation;
        transform.position = target.position + offset; // Adjust the height of the health bar
    }
}
