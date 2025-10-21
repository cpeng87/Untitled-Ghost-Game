using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TongsController : MinigameCompletion
{
    float speed = 3f;
    float objectHalfWidth = 1f;
    bool isGrabDonut = false;
    Transform grabbedDonut;
    Vector3 originalPos;
    float dropHeight = 2f; // Tongs move down by this distance
    bool leftHit = false;
    bool rightHit = false;
    GameObject leftHitObj = null;
    GameObject rightHitObj = null;
    Camera cam;
    Vector3 camLeftBound;
    Vector3 camRightBound;
    List<ContactPoint> contactPoints;
    private bool isGrabbing;
    private bool isDown;
    [SerializeField] private BoxCollider leftTong;
    [SerializeField] private BoxCollider rightTong;
    private int attempts = 0;
    [SerializeField] private List<GameObject> lives;

    void Start()
    {
        contactPoints = new List<ContactPoint>(10); // Preallocated
        originalPos = transform.position;

        // Get camera bounds
        cam = Camera.main;
        camLeftBound = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        camRightBound = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
    }

    void OnCollisionEnter(Collision collision)
    {
        contactPoints.Clear();
        collision.GetContacts(contactPoints);

        foreach (ContactPoint contact in contactPoints)
        {
            Transform hitPart = contact.thisCollider.transform;
            if (hitPart.CompareTag("LeftTong"))
            {
                leftHit = true;
                leftHitObj = collision.gameObject;
            }
            else if (hitPart.CompareTag("RightTong"))
            {
                rightHit = true;
                rightHitObj = collision.gameObject;
            }
        }
        // Pick up the donut only if it's between the left and right ends of tong, and both ends hit the same donut
        if (leftHit && rightHit && leftHitObj == rightHitObj && !isGrabDonut)
        {
            grabbedDonut = collision.gameObject.transform;
            grabbedDonut.SetParent(transform);
            grabbedDonut.gameObject.GetComponent<Rigidbody>().useGravity = false;
            grabbedDonut.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            isGrabDonut = true;
        }
    }

    public IEnumerator Grab()
    {
        Vector3 startPos = transform.position;
        Vector3 dropPos = new Vector3(startPos.x, dropHeight, startPos.z);
        isGrabbing = true;

        // Move down
        while (transform.position.y > dropHeight)
        {
            transform.position = Vector3.MoveTowards(transform.position, dropPos, speed * Time.deltaTime);
            if (isGrabDonut)
            {
                AudioManager.Instance.PlaySound("Grab");
                break;
            }
            yield return null;
        }

        leftTong.enabled = false;
        rightTong.enabled = false;

        // Move back up
        Vector3 upPos = new Vector3(startPos.x, originalPos.y, startPos.z);
        while (transform.position.y < originalPos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, upPos, speed * Time.deltaTime);
            yield return null;
        }
        CheckCompletion();
        leftHit = rightHit = false;
        leftHitObj = rightHitObj = null;
        leftTong.enabled = true;
        rightTong.enabled = true;
        isGrabbing = false;
    }

    void Update()
    {

        float input = Input.GetAxis("Horizontal");

        // Horizontal movement
        if (input != 0 && !isGrabbing)
        {
            Vector3 move = new Vector3(input * speed * Time.deltaTime, 0f, 0f);
            transform.position += move;

            // Clamp position within camera bounds
            Vector3 pos = transform.position;
            float minX = camLeftBound.x + objectHalfWidth;
            float maxX = camRightBound.x - objectHalfWidth;
            pos.x = Mathf.Clamp(transform.position.x, minX, maxX);
            transform.position = pos;
        }

        // Start grabbing donut
        if (!isGrabbing && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Grab());
        }

        // // Reset variables if failed to grab donut
        // if (transform.position == originalPos)
        // {
        //     leftHit = rightHit = false;
        //     leftHitObj = rightHitObj = null;
        // }
    }

    private void LoseLife(int attempt)
    {
        lives[3 - attempt].SetActive(false);
    }

    private void CheckCompletion()
    {
        if (isGrabDonut)
        {
            minigameResult.MinigameResult(true);
        }
        else
        {
            attempts += 1;
            LoseLife(attempts);
            if (attempts >= 3)
            {
                minigameResult.MinigameResult(false);
            }
        }
    }
}
