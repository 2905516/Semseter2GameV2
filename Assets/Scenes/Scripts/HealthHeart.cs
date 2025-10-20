using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    private Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartState(HeartState state)
    {
        switch (state)
        {
            case HeartState.Full:
                heartImage.sprite = fullHeart;
                break;
            case HeartState.Half:
                heartImage.sprite = halfHeart;
                break;
            case HeartState.Empty:
                heartImage.sprite = emptyHeart;
                break;
        }
    }
}

public enum HeartState
{
    Full = 2,
    Half = 1,
    Empty = 0
}
