using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;

    // Placeholders: list of icons that are used in the OptionsMeny game object
    [SerializeField] private RawImage masterOnIcon;
    [SerializeField] private RawImage masterOffIcon;
    [SerializeField] private RawImage musicOnIcon;
    [SerializeField] private RawImage musicOffIcon;

    private bool masterMuted = false;
    private bool musicMuted = false;

    private void Start()
    {
        LoadVolume();

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);

        masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);

        UpdateIcons();
    }

    public void SetMasterVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        audioMixer.SetFloat("master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        float masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;

        masterMuted = PlayerPrefs.GetInt("masterMuted", 0) == 1;
        musicMuted = PlayerPrefs.GetInt("musicMuted", 0) == 1;

        if (masterMuted)
        {
            audioMixer.SetFloat("master", -80f);
        }
        else
        {
            audioMixer.SetFloat("master", Mathf.Log10(masterVolume) * 20);
        }

        if (musicMuted)
        {
            audioMixer.SetFloat("music", -80f);
        }
        else
        {
            audioMixer.SetFloat("music", Mathf.Log10(musicVolume) * 20);
        }

        UpdateIcons();
    }

    public void OnMasterButtonPress() 
    {
        masterMuted = !masterMuted;
        if (masterMuted)
        {
            audioMixer.SetFloat("master", -80f);
        }
        else
        {
            float volume = PlayerPrefs.GetFloat("masterVolume", 1f);
            audioMixer.SetFloat("master", Mathf.Log10(volume) * 20);
        }

        PlayerPrefs.SetInt("masterMuted", masterMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateIcons();
    }

    public void OnMusicButtonPress() 
    {
        musicMuted = !musicMuted;
        if (musicMuted)
        {
            audioMixer.SetFloat("music", -80f);
        }
        else
        {
            float volume = PlayerPrefs.GetFloat("musicVolume", 1f);
            audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        }

        PlayerPrefs.SetInt("musicMuted", musicMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateIcons();
    }

    private void UpdateIcons()
    {
        masterOnIcon.enabled = !masterMuted;
        masterOffIcon.enabled = masterMuted;

        musicOnIcon.enabled = !musicMuted;
        musicOffIcon.enabled = musicMuted;
    }
}