using UnityEngine;
using System.Collections;

public class SubmitPancakes : MonoBehaviour
{
    private static int pancakes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void onFinish(bool failed)
    {
        GameObject[] allPancakes = GameObject.FindGameObjectsWithTag("Pancake");
        pancakes = allPancakes.Length - 1;
        bool result;
        if (pancakes < 10 || failed)
        {
            //set to main scene
            //fail dialogue
            Debug.Log("You failed!");
            result = false;
        } else
        {
            //set to main scene
            //success dialogue
            Debug.Log("You succeeded!");
            result = true;
        }
        GameManager.Instance.CompleteMinigame(result);
    }    
}
