using System.Collections;
using UnityEngine;

public class DonutGameManager : MonoBehaviour
{
    public Transform tongs;           // Reference to the tongs GameObject
    public Transform tray;            // Reference to the tray where donuts are placed
    public GameObject donutPrefab;    // Reference to the donut prefab
    public float moveSpeed = 5f;      // Speed at which the tongs can move horizontally
    public float dropHeight = 5f;     // How far down the tongs will drop to grab a donut
    public float grabHeight = 1f;     // Height where the tongs will stop grabbing

    private bool isMoving = false;    // Flag to prevent movement while dropping
    private Vector3 initialTongsPos;  // The initial position of the tongs

    void Start()
    {
        initialTongsPos = tongs.position;  // Save initial position
    }

    void Update()
    {
        if (!isMoving)
        {
            // Move tongs left or right
            float move = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            tongs.position = new Vector3(Mathf.Clamp(tongs.position.x + move, tray.position.x - 5f, tray.position.x + 5f), tongs.position.y, tongs.position.z);
        }

        // Press "Space" to drop the tongs and try to grab a donut
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DropAndGrab());
        }
    }

    IEnumerator DropAndGrab()
    {
        isMoving = true;

        // Move the tongs downwards
        Vector3 targetPosition = tongs.position - new Vector3(0, dropHeight, 0);
        float dropDuration = 1f;
        float elapsedTime = 0;

        while (elapsedTime < dropDuration)
        {
            tongs.position = Vector3.Lerp(tongs.position, targetPosition, (elapsedTime / dropDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Check if the tongs grabbed a donut
        RaycastHit hit;
        if (Physics.Raycast(tongs.position, Vector3.down, out hit, 1f))
        {
            if (hit.collider.CompareTag("Donut"))
            {
                hit.collider.transform.SetParent(tongs);
                hit.collider.GetComponent<Rigidbody>().isKinematic = true; // Disable physics for the grabbed donut
            }
        }

        // Move the tongs up with the donut
        targetPosition = initialTongsPos;
        elapsedTime = 0;

        while (elapsedTime < dropDuration)
        {
            tongs.position = Vector3.Lerp(tongs.position, targetPosition, (elapsedTime / dropDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Check if the donut was grabbed correctly
        if (tongs.childCount > 0)
        {
            // Successfully grabbed donut
            Debug.Log("Donut grabbed!");
        }
        else
        {
            // Failed to grab donut
            Debug.Log("Failed to grab donut.");
        }

        // Reset tongs to the initial position
        tongs.position = initialTongsPos;
        isMoving = false;
    }
}
