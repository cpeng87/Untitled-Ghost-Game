using UnityEngine;

public class BobaCollision : MonoBehaviour
{
    private Rigidbody rb;

    private bool isTriggered = false; //ensures boba doesn't infinitely update the counter 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0;
    }
    private void FixedUpdate() //fix the physics 
    {
        rb.WakeUp();
    }
    private void OnTriggerEnter(Collider collider)
    {
        GameObject potPrefab = GameObject.Find("Pot");
        GameObject boundsPrefab = GameObject.Find("StrainerAndBounds/Strainer/Bounds");

        if (collider.gameObject.name.Equals(boundsPrefab.name) && !isTriggered) //if the boba hits the bounds collider...
        {
            isTriggered = true;
            //Debug.Log("BobaHitStrainer - " + collider.gameObject.name);
            var script = potPrefab.GetComponent<BobaPotController>();
            script.OnBobaAdd(); // update boba counter
            AudioManager.Instance.PlaySound("Boba");
        } 
    }

    private void OnTriggerExit(Collider collider)
    {
        GameObject potPrefab = GameObject.Find("Pot");
        GameObject boundsPrefab = GameObject.Find("StrainerAndBounds/Strainer/Bounds");
        if (collider.gameObject.name.Equals(boundsPrefab.name)) //if the boba leaves the bounds collider...
        {
            if (transform.position.y > boundsPrefab.transform.position.y) { //only if the boba is above the collider!
                isTriggered = false;
                //Debug.Log("BobaLeftStrainer");
                var script = potPrefab.GetComponent<BobaPotController>();
                script.OnBobaRemove(); //update boba counter
            }
        } 
    }
}
