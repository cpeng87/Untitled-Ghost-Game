using UnityEngine;
using System.Collections.Generic;

public class StoryObj : MonoBehaviour
{
    // [SerializeField] private GameObject item;
    [SerializeField] private List<string> ghostNames;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (string ghostName in ghostNames)
        {
            Ghost ghost = GameManager.Instance.ghostManager.GetGhostScriptableFromName(ghostName);
            if (ghost )
            if (GameManager.Instance.ghostManager.IsComplete(ghost) == false)
            {
                this.gameObject.SetActive(false);
                return;
            }
        }
        this.gameObject.SetActive(true);
    }
}
