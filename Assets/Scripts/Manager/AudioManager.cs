using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private Music[] music;
    [SerializeField] private Sound[] sound;
    private Dictionary<string, AudioClip> musicDict;
    private Dictionary<string, AudioClip> soundDict;
    private string currentSong;
    private int currIndex = 0;
    private string currentScene;
    private string savedSong;
    private float savedTime;
    

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource; //We may want to pool audio sources or switch to a different method
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Coroutine currentFade;

    public static Action<string> OnSongChanged;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Instance.MergeAudio(this);
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        musicDict = new Dictionary<string, AudioClip>();
        soundDict = new Dictionary<string, AudioClip>();
        if (musicSource == null) Debug.LogError("Music source in AudioManager is null");
        foreach (Music m in music) musicDict.Add(m.name, m.clip);
        foreach (Sound s in sound) soundDict.Add(s.name, s.clip);
    }
    void Start()
    {
        //Random Song for now
        // RandomSong();
        //change to play arc's song
        // if (GameManager.Instance == null)
        // {
        //     musicSource.Stop();
        //     musicSource.clip = music[currIndex].clip;
        //     musicSource.Play();
        // }
        // else
        // {
        //     int arcIndex = (int)GameManager.Instance.arc - 1;
        //     if (arcIndex < 0 || arcIndex >= music.Length)
        //     {
        //         arcIndex = 0;
        //     }
        //     musicSource.Stop();
        //     if (music.Length > arcIndex)
        //     {
        //         musicSource.clip = music[arcIndex].clip;
        //         musicSource.Play();
        //         OnSongChanged?.Invoke(music[arcIndex].name);
        //     }
        // }
    }

    /// <summary>
    /// Changes the song that's currently playing.
    /// </summary>
    /// <param name="songName"></param>
    public void PlaySong(string songName)
    {
        if (musicDict.TryGetValue(songName, out var clip))
        {
            if (musicSource.clip == clip) return; //Already playing
            currentSong = songName;
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.Play();
            OnSongChanged?.Invoke(songName);
        }
        else
        {
            Debug.LogWarning("Song " + songName + " not found!");
        }
    }

    /// <summary>
    /// Changes the song that's currently playing but now with fade
    /// </summary>
    /// <param name="songName"></param>
    public void PlaySong(string songName, float fadeTime = 1f) {

        if (musicDict.TryGetValue(songName, out var clip))  {
            if (musicSource.clip == clip && musicSource.isPlaying)
                return; // already playing

            currentSong = songName;

            if (currentFade != null) StopCoroutine(currentFade);
            currentFade = StartCoroutine(FadeToNewTrack(clip, fadeTime));

            OnSongChanged?.Invoke(songName);
        }
        else  {
            Debug.LogWarning("Song " + songName + " not found!");
        }
    }

    public void SaveSong()
    {
       savedTime = musicSource.time;
       savedSong = currentSong;
    }

    public bool ContinueSong()
    {
        if (savedSong != null)
        {
            musicSource.clip = musicDict[savedSong];
            musicSource.time = savedTime;
            musicSource.Play();
        }
        else
        {
            return false;
        }

        savedSong = null;
        savedTime = 0;

        return true;
    }

    private System.Collections.IEnumerator FadeToNewTrack(AudioClip newClip, float fadeTime)   {
    float startVol = musicSource.volume;

    // fade out
    for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)   {
        musicSource.volume = Mathf.Lerp(startVol, 0, t / fadeTime);
        yield return null;
    }

    musicSource.Stop();
    musicSource.clip = newClip;
    musicSource.Play();

    // fade in
    for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)   {
        musicSource.volume = Mathf.Lerp(0, startVol, t / fadeTime);
        yield return null;
    }

    musicSource.volume = startVol;
}

    public void StopSong()
    {
        musicSource.Stop();
    }

    public void MergeAudio(AudioManager other)
{
    foreach (var m in other.music)  {
        if (!musicDict.ContainsKey(m.name))
        {
            musicDict[m.name] = m.clip;
        }
    }

    foreach (var s in other.sound)  {
        if (!soundDict.ContainsKey(s.name))
        {
            soundDict[s.name] = s.clip;
        }
    }
}


    public void RandomSong()
    {
        //Get a song index that's different than what's already playing
        int index = Random.Range(0, music.Length);
        while (music[index].name == currentSong) index = Random.Range(0, music.Length);
        currentSong = music[index].name;
        currIndex = index;
        musicSource.Stop();
        musicSource.clip = music[index].clip;
        musicSource.Play();
        //OnSongChanged?.Invoke(CurrentSong);
        OnSongChanged?.Invoke(music[index].name);
    }

    public void NextSong()
    {
        //pays the next song on the list
        currIndex = (currIndex + 1) % music.Length;
        Debug.Log("Music Index: " + currIndex);
        currentSong = music[currIndex].name;
        musicSource.Stop();
        musicSource.clip = music[currIndex].clip;
        musicSource.Play();
        OnSongChanged?.Invoke(music[currIndex].name);
    }
    public void NextSong(int index)
    {
        if (index < music.Length)
        {
            currIndex = index;
            currentSong = music[currIndex].name;
            musicSource.Stop();
            musicSource.clip = music[currIndex].clip;
            musicSource.Play();
            OnSongChanged?.Invoke(music[currIndex].name);
        }
    }

    /// <summary>
    /// Play a sound effect. Chen: This jit ain't tested at all lmao
    /// Theoretically PlayOnShot can play multiple sounds without interrupting another one?
    /// May want to introduce pitch variation
    /// </summary>
    public void PlaySound(string soundName)
    {
        if (soundDict.TryGetValue(soundName, out var clip))
        {
            soundSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Song " + soundName + " not found!");
        }
    }
    public bool CheckPlaying()
    {
        return soundSource.isPlaying;
    }

    public void SetSoundLooping(bool isLooping)
    {
        soundSource.loop = isLooping;
    }

    public void StopSound()
    {
        soundSource.Stop();
    }

    public void PauseSound()
    {
        soundSource.Pause();
    }

    public void UnPauseSound()
    {
        soundSource.UnPause();
    }

    public void PlaySound(AudioClip sound)
    {
        if (sound == null)
        {
            Debug.Log("Sound to play is null");
            return;
        }
        soundSource.PlayOneShot(sound);
    }

    public void SetReaperPitch(bool val)
    {
        if (val)
        {
            soundSource.pitch = 0.75f;
        }
        else
        {
            soundSource.pitch = 1f;
        }
    }

    public int GetMusicCount() => music.Length;
    public string GetMusicName(int index) => index >= 0 && index < music.Length ? music[index].name : null;

}
