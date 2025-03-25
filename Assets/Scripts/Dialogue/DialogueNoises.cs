// using UnityEngine;
// using Yarn.Unity;
// using System.Collections.Generic;

// public class DialogueNoises : MonoBehaviour
// {
//     public LineView lineView;
//     [SerializeField] private List<AudioClip> letterClips;
//     private Dictionary<string, AudioClip> letterDict;

//     private float cooldownTime = 0.1f; // Time in seconds between playing each sound
//     private float lastPlayedTime = 0f; // Tracks the last time a sound was played

//     void Start()
//     {
//         // Initialize the dictionary with audio clips
//         letterDict = new Dictionary<string, AudioClip>();
//         foreach (AudioClip letter in letterClips)
//         {
//             letterDict.Add(letter.name, letter);
//         }

//         // Subscribe to the onCharacterTyped event
//         lineView.onCharacterTyped.AddListener(PlayLetter);
//     }

//     private void OnDestroy()
//     {
//         // Unsubscribe to prevent memory leaks
//         lineView.onCharacterTyped.RemoveListener(PlayLetter);
//     }

//     private void PlayLetter(char typedCharacter)
//     {
//         // Check if enough time has passed since the last sound was played
//         if (Time.time - lastPlayedTime >= cooldownTime)
//         {
//             string character = typedCharacter.ToString().ToLower();

//             // Play the corresponding sound if it exists in the dictionary
//             if (letterDict.TryGetValue(character, out AudioClip clip))
//             {
//                 AudioManager.Instance.PlaySound(clip);
//                 lastPlayedTime = Time.time; // Update the last played time
//             }
//         }
//     }
// }


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
