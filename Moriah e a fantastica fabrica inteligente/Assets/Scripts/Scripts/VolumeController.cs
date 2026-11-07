using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private const string MusicVolumeKey = "MusicVolume";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey(MusicVolumeKey))
        {
            float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey);
            volumeSlider.value = savedVolume;
            SetVolume(savedVolume);
        }
        else
        {
            SetVolume(volumeSlider.value);
        }

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float sliderValue)
    {
        float volumeInDb = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;

        audioMixer.SetFloat("MusicVolume", volumeInDb);

        PlayerPrefs.SetFloat(MusicVolumeKey, sliderValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
