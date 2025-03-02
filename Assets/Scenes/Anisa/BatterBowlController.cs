using UnityEngine;

public class BatterBowlController : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private bool isTilting = false;
    private float tiltAngle = 0f;
    private float originalZ;
    [Header("Tilt Settings")]
    public float tiltSensitivity = 10f;
    public float maxTiltAngle = 40f;
    [Header("Bounds for Dragging")]
    public Vector2 xBound;
    public Vector2 yBound;
    public Vector2 zBound;
    [Header("Particle System")]
    public ParticleSystem pourParticles;
    public float pourThreshold = 20f;
    [Header("Muffin Fill Parameters")]
    private float maxFillHeight = 0.4f; // The max amount of batter that can be filled
    private Collider tiltCollider;

    void Start()
    {
        originalZ = transform.position.z;
    }

    //takes in point clicked and moves the bowl to the location dragged to
    private void HandleMoving()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point + offset;

            float clampedX = Mathf.Clamp(targetPosition.x, xBound.x, xBound.y);
            float clampedY = Mathf.Clamp(targetPosition.y, yBound.x, yBound.y);
            float clampedZ = Mathf.Clamp(targetPosition.z, zBound.x, zBound.y);

            transform.position = new Vector3(clampedX, clampedY, clampedZ); //, originalZ);
        }
    }

    // tilts the bowl based on the mouse's y movement. Limited by max tilt angle.
    private void HandleTilting()
    {
        float mouseDeltaX = Input.GetAxis("Mouse Y") * -1; // Flip the direction
        tiltAngle += mouseDeltaX * tiltSensitivity;
        tiltAngle = Mathf.Clamp(tiltAngle, -maxTiltAngle, maxTiltAngle);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, tiltAngle);
    }

    // Fill individual muffin cups by using a plane and updating its height to simulate filling process using particles
    void FillMuffinCup(Collider cupCollider, FillTracker tracker) {
        Transform fillPlane = cupCollider.transform.GetChild(1);
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[pourParticles.particleCount];
        int numParticles = pourParticles.GetParticles(particles);

        foreach (var particle in particles)
        {
            // Check if the particle is inside the cup
            if (cupCollider.bounds.Contains(particle.position))
            {
                // Increase fill level based on particles in the cup
                tracker.currentFillLevel += Time.deltaTime * 0.01f;
                tracker.currentFillLevel = Mathf.Clamp01(tracker.currentFillLevel); // Ensure fill level is between 0 and 1
                UpdateFillProgress(tracker, tracker.currentFillLevel, numParticles);
            }
        }

        // Update the fill plane's position based on fill level
        Vector3 newFillPosition = fillPlane.localPosition;
        newFillPosition.y = Mathf.Lerp(0f, maxFillHeight, tracker.currentFillLevel);
        fillPlane.localPosition = newFillPosition;
    }

    // Update individual muffin cup's progress slider and fill state
    void UpdateFillProgress(FillTracker tracker, float currentLevel, int numParticles) {
        var progressBar = tracker.GetProgressBar();

        // Cup will be full only if min amount of batter was added upto certain height
        float minParticleCount = 100;
        progressBar.value = Mathf.Min(currentLevel / maxFillHeight, (float)numParticles / minParticleCount);

        if (progressBar.value >= 1.0f && numParticles >= minParticleCount)
        {
            Debug.Log("Cup is full!");
            tracker.isFull = true;
        } 
        else 
        {
            tracker.isFull = false;
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log(hit.collider);
                //clicked on main body collider, begin moving the bowl
                if (hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    offset = transform.position - hit.point;
                }
                //clicked left side of bowl, begin tilting it
                else if (hit.collider.CompareTag("Tilt"))
                {
                    Debug.Log("checking if we hit tilt");
                    isTilting = true;
                    tiltCollider = hit.collider;
                }
            }
        }
        
        if (Input.GetMouseButton(0))
        {
            if (isDragging)
            {
                HandleMoving();
            }
            else if (isTilting)
            {
                HandleTilting();
            }
        }

        //no longer dragging
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            isTilting = false;
        }

        // Check tilt 
        if (tiltAngle > pourThreshold)
        {
            // Check if there is a muffin cup under the bowl to pour batter
            Vector3 rayOrigin = tiltCollider.bounds.center;
            Vector3 rayDirection = -transform.right;
            Debug.DrawRay(rayOrigin, rayDirection * 10f, Color.yellow); // Debug code

            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit)) {

                // If the object is muffin tin, begin pouring particles until it is full
                if (hit.collider.CompareTag("MuffinTin")) {
                    Debug.Log("start pouring!"); 
                    var fillTracker = hit.collider.GetComponent<FillTracker>();
                    if (!fillTracker.isFull) {
                        if (!pourParticles.isEmitting)
                        {
                            pourParticles.Play();
                        }
                        FillMuffinCup(hit.collider, fillTracker);
                    }
                } else {
                    if (pourParticles.isEmitting)
                    {
                        pourParticles.Stop();
                    }
                }
            }
        }
        else
        {
            if (pourParticles.isEmitting)
            {
                pourParticles.Stop();
            }
        }
    }
}
