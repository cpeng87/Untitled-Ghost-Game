using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInstructions : MonoBehaviour
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
        if (active == false && GameManager.Instance != null && GameManager.Instance.GetTutorialState("Game") == false)
        {
            if (GhostSpawningManager.Instance.GetSpawnedGhost(0) == null)
            {
                return;
            }
            else if (GhostSpawningManager.Instance.GetSpawnedGhost(0).GetComponent<GhostObj>().GetState() == GhostState.CanTakeOrder)
            {
                StartStep();
                active = true;
            }
        }
        else if (active)
        {
            if (index == 0)
            {
                // this be hardcoded to check if reaper order is ready to be taken
                if (GhostSpawningManager.Instance.GetSpawnedGhost(0).GetComponent<GhostObj>().GetState() == GhostState.CanTakeOrder)
                {
                    StartStep();
                }
            }
            CheckStepComplete();
        }
    }

    public void StartStep() {
        index = 0;
        steps[index].SetActive(true);
    }

    private void CheckStepComplete()
    {
        bool stepComplete = false;
        if (index == 0)
        {
            stepComplete = GameManager.Instance.state == State.Dialogue;
            Debug.Log(stepComplete);
        }
        else if (index == 1)
        {
            stepComplete = GameManager.Instance.state == State.Main;
        }
        else if (index == 2)
        {
            stepComplete = TicketManager.Instance.IsTicketsActive();
        }
        if (stepComplete)
        {
            Progress();
        }
    }


    private void Progress()
    {
        index += 1;
        if (index >= steps.Count)
        {
            steps[index - 1].SetActive(false);
            GameManager.Instance.SetTutorialState("Game", true);   // done with tutorial
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
