using UnityEngine;
using System.Collections;

public class SubmitPancakes : MonoBehaviour
{
    private static int pancakes;
    public static bool failed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void onFinish()
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
}
