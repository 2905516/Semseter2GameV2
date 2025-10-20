using UnityEngine;
using UnityEngine.UI;


public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager instance;
    private static AudioSource audioSource;
    private static SoundEffectLibary SoundEffectLibary;
    private static AudioSource voiceAudioSource;
    [SerializeField] private Slider sfxslider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource = audioSources[0];
            voiceAudioSource = audioSources[1];
            SoundEffectLibary = GetComponent<SoundEffectLibary>();
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);

        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = SoundEffectLibary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);


        }



    }

    public static void PlayVoice(AudioClip audioClip, float pitch = 1f)
    {
        voiceAudioSource.pitch = pitch;
        voiceAudioSource.PlayOneShot(audioClip);



    }

  /*  private void Update()
    {
        if (Input.anyKeyDown)
            Debug.Log($"[InputTest] Key pressed: {Input.inputString}");
    } */

    private void Start()
    {
        sfxslider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
        voiceAudioSource.volume = volume;


    }

    public void OnValueChanged()
    {

        SetVolume(sfxslider.value);

    }
}
