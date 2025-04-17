using UnityEngine;
using System.Collections;

public class SubmitPancakes : MonoBehaviour
{
    private int pancakes;
    public bool failed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void onFinish()
    {
        GameObject[] allPancakes = GameObject.FindGameObjectsWithTag("Pancake");
        pancakes = allPancakes.Length - 1;
        bool result = false;
        if (pancakes < 10 || failed)
        {
            //set to main scene
            //fail dialogue
            result = false;
        } else
        {
            //set to main scene
            //success dialogue
            result = true;
        }
       GameManager.Instance.CompleteMinigame(result);
    }
    private void OnTriggerEnter(Collider other)
    {
        //do something to fail
        failed = true;
    }

    public void GiveUpMinigame()
    {
        GameManager.Instance.CompleteMinigame(false);
    }
}
