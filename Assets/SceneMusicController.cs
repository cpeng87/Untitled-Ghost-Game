using UnityEngine;

public class SceneMusicController : MonoBehaviour
{
    [SerializeField] private string songName;   // scene song name
    [SerializeField] private float fadeTime = 1f;

    private void Start()
    {
        if (AudioManager.Instance != null && !string.IsNullOrEmpty(songName))
        {
            AudioManager.Instance.PlaySong(songName, fadeTime);
        }
    }
}

