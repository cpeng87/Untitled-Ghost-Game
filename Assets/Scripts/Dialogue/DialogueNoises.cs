using UnityEngine;
using Yarn.Unity;
using System.Collections.Generic;

public class DialogueNoises : MonoBehaviour
{
    public LineView lineView;
    // public AudioManager audioManager;
    [SerializeField] private List<AudioClip> letterClips;
    private Dictionary<string, AudioClip> letterDict;

    [SerializeField] private float cooldownTime = 0.1f; // Time in seconds between playing each sound
    private float lastPlayedTime = 0f; // Tracks the last time a sound was played

    void Start()
    {
        letterDict = new Dictionary<string, AudioClip>();
        foreach (AudioClip letter in letterClips)
        {
            letterDict.Add(letter.name, letter);
        }
        lineView.onCharacterTyped.AddListener(PlayLetter);
    }

    private void PlayLetter()
    {
        if (Time.time - lastPlayedTime >= cooldownTime)
        {
            int index = (int) (Random.value * letterClips.Count);
            AudioManager.Instance.PlaySound(letterClips[index]);
            lastPlayedTime = Time.time;
        }
    }
}
