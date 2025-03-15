using UnityEngine;

public class ReaperM2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //THIS SCRIPT JUST MAKES THE REAPER MOVE UP, AND THEN COME BACK DOWN TO ITS STARTING POSITION
        
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.005f, transform.position.z);
        if (transform.position.y > 30)
        {
            transform.position = new Vector3(transform.position.x, -5, transform.position.z);
        }
        
    }
}
