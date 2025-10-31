using UnityEngine;
using System.Collections;

public class SubmitPancakes : MinigameCompletion
{
    private int pancakes;
    public bool failed = false;
    private int successPancakes = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void onFinish()
    {
        GameObject[] allPancakes = GameObject.FindGameObjectsWithTag("Pancake");
        pancakes = allPancakes.Length - 1;
        bool result = false;

        foreach (GameObject pancake in allPancakes)
        {
            //check each pancake x value (between -3 and 4 (plate area)) and add to success counter
            if (pancake.transform.position.x > -3f && pancake.transform.position.x < 4f)
            {
                successPancakes++;
            }

        }


        if (successPancakes < 6 || failed)
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
        minigameResult.MinigameResult(result);
    }
    private void OnTriggerEnter(Collider other)
    {
        //do something to fail
        minigameResult.MinigameResult(false);
    }
}
