using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoryInstructions : MonoBehaviour
{
    [SerializeField] private List<GameObject> steps = new List<GameObject>();
    private int index;
    private bool active;

    private void Start()
    {
        foreach (GameObject step in steps)
        {
            step.SetActive(false);
        }

    }

    private void Update()
    {
        if (active == false && GameManager.Instance != null && GameManager.Instance.GetTutorialState("Game") == true && GameManager.Instance.GetTutorialState("Story") == false)
        {
            if (GameManager.Instance.state == State.Main && GhostSpawningManager.Instance.GetSpawnedGhost(0) == null)
            {
                StartStep();
                active = true;
            }
        }
        else if (active == true && Input.GetMouseButtonUp(0))
        {
            CheckStepComplete();
        }
    }

    public void StartStep() {
        index = 0;
        steps[index].SetActive(true);
    }

    private void CheckStepComplete()
    {
        //do nothing
        Progress();
    }


    private void Progress()
    {
        index += 1;
        if (index >= steps.Count)
        {
            steps[index - 1].SetActive(false);
            GameManager.Instance.SetTutorialState("Story", true);   // done with tutorial
            active = false;
            return;
        }
        else
        {
            steps[index - 1].SetActive(false);
            steps[index].SetActive(true);
        }
    }
}
