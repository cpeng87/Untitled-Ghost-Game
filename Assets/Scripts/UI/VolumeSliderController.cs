using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    [SerializeField] private Slider master;
    [SerializeField] private Slider background;
    [SerializeField] private Slider sfx;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        master.value = audioManager.GetMasterVolume();
        background.value = audioManager.GetMusicSource().volume;
        sfx.value = audioManager.GetSoundSource().volume;
    }

    private void Update()
    {
        Save();
    }

    private void Save()
    {
        audioManager.SetMasterVolume(master.value);
        audioManager.GetMusicSource().volume = master.value * background.value;
        audioManager.GetSoundSource().volume = master.value * sfx.value;
    }
}

// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;

// public class OptionsController : MonoBehaviour {

// 	public Slider volumeSlider;
// 	public Slider difficultySlider;
// 	public LevelManager levelManager;

// 	private MusicManager musicManager;
// 	// Use this for initialization
// 	void Start () {
// 		musicManager = GameObject.FindObjectOfType<MusicManager> ();

// 		volumeSlider.value = PlayerPrefsManager.GetMasterVolume ();
// 	}
	
// 	// Update is called once per frame
// 	void Update () {
		
// 	}

// 	public void SaveAndExit () {
// 		PlayerPrefsManager.SetMasterVolume (volumeSlider.value);
// 		levelManager.LoadLevel ("Start Menu");
// 	}
// }
