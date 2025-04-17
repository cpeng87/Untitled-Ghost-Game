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
    
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource; //We may want to pool audio sources or switch to a different method
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static Action<string> OnSongChanged;
    void Awake()
    {
        Instance = this;
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
        if (GameManager.Instance == null)
        {
            musicSource.Stop();
            musicSource.clip = music[currIndex].clip;
            musicSource.Play();
        }
        else
        {
            int arcIndex = (int) GameManager.Instance.arc - 1;
            if (arcIndex < 0)
            {
                arcIndex = 0;
            }
            musicSource.Stop();
            musicSource.clip = music[arcIndex].clip;
            musicSource.Play();
            OnSongChanged?.Invoke(music[arcIndex].name);
        }
    }
    
    /// <summary>
    /// Changes the song that's currently playing.
    /// </summary>
    /// <param name="songName"></param>
    public void PlaySong(string songName)
    {
        if (musicDict.TryGetValue(songName, out var clip))
        {
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

    public void PlaySound(AudioClip sound)
    {
        if (sound == null)
        {
            Debug.Log("Sound to play is null");
            return;
        }
        soundSource.PlayOneShot(sound);
    }
}
