using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderVolume : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string nomeParametro = "MusicVolume";

    private Slider volumeSlider;
    private const string VolumeKey = "SavedMusicVolume";

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        volumeSlider.minValue = 0.0001f;
        volumeSlider.maxValue = 1f;

        float volumeSalvo = PlayerPrefs.GetFloat(VolumeKey, volumeSlider.value);
        
        volumeSlider.value = volumeSalvo;
        DefinirAudioVolume(volumeSalvo);

        volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float valor)
    {
        DefinirAudioVolume(valor);
        PlayerPrefs.SetFloat(VolumeKey, valor);
        PlayerPrefs.Save();
    }

    private void DefinirAudioVolume(float valorSlider)
    {
        float volumeInDb = Mathf.Log10(Mathf.Clamp(valorSlider, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(nomeParametro, volumeInDb);
    }

    private void OnDestroy()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}
