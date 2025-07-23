using UnityEngine;
using UnityEngine.UI;

public class BobaPotController : MinigameCompletion
{
    private bool isTilting = false;
    private float tiltAngle = 0f;

    [Header("Tilt Settings")]
    public float tiltSensitivity = 10f;
    public float maxTiltAngle = 40f;

    [SerializeField] private int bobaCounter;
    public Slider bobaProgress;
    [SerializeField] private int neededBoba;
    private GameObject childCollider;

    void Start()
    {
        //set slider values based on neededparticles
        bobaProgress.minValue = 0;
        bobaProgress.maxValue = neededBoba;
        childCollider = GameObject.Find("Pot/Circle");
    }

    // void Update()
    // {

    //     if (GetComponent<BobaSpawner>().bobaDoneSpawning()) //only start moving if the boba is done spawning
    //     {
    //         if (Input.GetMouseButtonDown(0))
    //         {
    //             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //             if (Physics.Raycast(ray, out RaycastHit hit))
    //             {
    //                 Debug.Log(hit.collider.gameObject.name);
    //                 //clicked on main body collider, begin tilting the pot
    //                 if (hit.collider.gameObject == gameObject || hit.collider.gameObject == childCollider)
    //                 {
    //                     isTilting = true;
    //                     Debug.Log("TiltingOn");
    //                 }
    //             }
    //         }

    //         if (Input.GetMouseButton(0))
    //         {
    //             if (isTilting)
    //             {
    //                 HandleTilting();
    //             }
    //         }

    //         //no longer dragging
    //         if (Input.GetMouseButtonUp(0))
    //         {
    //             isTilting = false;
    //         }
    //     }
    // }

    // tilts the pot based on the mouse's y movement. Limited by max tilt angle.
    private void HandleTilting()
    {
        float mouseDeltaX = Input.GetAxis("Mouse Y");
        tiltAngle += mouseDeltaX * tiltSensitivity;
        tiltAngle = Mathf.Clamp(tiltAngle, -maxTiltAngle, maxTiltAngle);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, tiltAngle);
    }

    //when boba hits one of the trigger objs (strainer), boba counter increases yippee!!
    public void OnBobaAdd()
    {
        bobaCounter += 1;

        //update slider
        bobaProgress.value = bobaCounter;
        CheckResults();
    }

    //when boba bounces out of one of the trigger objs (strainer), boba counter decreases :(
    public void OnBobaRemove()
    {
        bobaCounter -= 1;

        //update slider
        bobaProgress.value = bobaCounter;
    }

    //ran when submit button is pressed. Verifies if amount of boba caught is greater than the needed to pass.
    //Completes the minigame and passes result to the gameManager.
    public void CheckResults()
    {
        // bool result = bobaCounter >= neededBoba;
        // if (result)
        // {
        //     Debug.Log("success");
        // }
        // else
        // {
        //     Debug.Log("fail");
        // }
        // GameManager.Instance.CompleteMinigame(result);
        bool result = bobaCounter >= neededBoba;
        if (result)
        {
            minigameResult.MinigameResult(true);
        }
    }

    public int getNeededBoba()
    {
        return neededBoba;
    }
}
