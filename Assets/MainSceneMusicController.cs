using UnityEngine;

public class MainSceneMusicController : MonoBehaviour
{
    [SerializeField] private float fadeTime = 1f;

    private void Start()  {
        if (AudioManager.Instance.ContinueSong())
        {
            return;
        }
        if (AudioManager.Instance != null)  {
            string songName;

            // pick based on arc
            if (GameManager.Instance != null)  {
                int arcIndex = (int)GameManager.Instance.arc - 1;
                if (arcIndex < 0) arcIndex = 0;

                if (arcIndex < AudioManager.Instance.GetMusicCount())
                    songName = AudioManager.Instance.GetMusicName(arcIndex);
                else
                    songName = AudioManager.Instance.GetMusicName(0);
            }
            else  {
                songName = AudioManager.Instance.GetMusicName(0);
            }

            AudioManager.Instance.PlaySong(songName, fadeTime);
        }
    }
}
