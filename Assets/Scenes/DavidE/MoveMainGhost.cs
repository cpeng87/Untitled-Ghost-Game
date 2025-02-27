using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MoveMainGhost : MonoBehaviour
{

    public GameObject customer;
    public GameObject mcGhost;

    public float speed;
    public float turnSpeed;
    
    private bool moving = false;
    private bool turning = false;
    private bool finished = false;
    private GameObject adjustPos;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) {
             //use vector3.moveTowards() on the mcGhost to the new object
            Rigidbody body = mcGhost.GetComponent<Rigidbody>();
            Rigidbody realGoal = adjustPos.GetComponent<Rigidbody>();
            body.position = Vector3.MoveTowards(body.position, realGoal.position, speed);
            if (body.position.Equals(realGoal.position)) {
                moving = false;
                turning = true;
            }
            
        
        } else if (turning) {
            Rigidbody body = mcGhost.GetComponent<Rigidbody>();
            Rigidbody goal = customer.GetComponent<Rigidbody>();

            Vector3 direction = goal.transform.position - body.transform.position;
            direction.y = 0;

            if (direction != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(-direction) * Quaternion.Euler(0, -90, 0);
                body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, targetRotation, turnSpeed);

                if (Quaternion.Angle(body.transform.rotation, goal.transform.rotation) < 5f) {
                    Vector3 lookRotation = Vector3.RotateTowards(body.transform.position, goal.transform.position, .01f, 1f);
                    turning = false;
                    finished = true;
                    cleanUp();
                }
            }
            
        }
        
    }

    void OnMouseDown() {
        if (!moving && !turning && !finished) {
            print("Test");
            adjustPos = new GameObject();
            Rigidbody body = mcGhost.GetComponent<Rigidbody>();
            Rigidbody goal = customer.GetComponent<Rigidbody>();

            float distX = math.abs(body.transform.localPosition.x - goal.transform.localPosition.x);
            float distZ = math.abs(body.transform.localPosition.z - goal.transform.localPosition.z);
            //If I feel like it, I'll re-implement the "MC Ghost goes to the most convenient spot" thing I was going for

        
            adjustPos.AddComponent<Rigidbody>();
            adjustPos.GetComponent<Rigidbody>().transform.localPosition = new Vector3(goal.transform.localPosition.x + 3, goal.transform.localPosition.y, goal.transform.localPosition.z);
            adjustPos.GetComponent<Rigidbody>().useGravity = false;
            
            Vector3 direction = adjustPos.GetComponent<Rigidbody>().transform.position;
            direction = new Vector3(direction.x, 0, direction.z);
            body.transform.LookAt(direction);
            body.transform.rotation = body.transform.rotation * Quaternion.Euler(0, 90, 0);
            
            moving = true;
        }
    }

    void cleanUp() {
        Destroy(adjustPos);
        //Start the minigame here!
    }

}
