using UnityEngine;

public class BatterBowlController : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private bool isTilting = false;
    public float tiltAngle = 0f;
    private float originalZ;
    private float originalY;
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
    public GameObject rayPoint;
    // public Collider tiltCollider;

    void Start()
    {
        originalZ = transform.position.z;
        originalY = transform.position.y;
        pourParticles.Stop();
    }

    //takes in point clicked and moves the bowl to the location dragged to
    private void HandleMoving()
    {
        // Cast a ray from the mouse position into the 3D world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Use the hit point's x and z, but override y with originalY
            Vector3 targetPosition = hit.point + offset;

            // Clamp x and z values within bounds
            float clampedX = Mathf.Clamp(targetPosition.x, xBound.x, xBound.y);
            float clampedY = Mathf.Clamp(targetPosition.y, yBound.x, yBound.y);

            // Set the position while keeping the y position fixed at originalY
            transform.position = new Vector3(clampedX, clampedY, originalZ);
        }
    }
    // tilts the bowl based on the mouse's y movement. Limited by max tilt angle.
    // private void HandleTilting()
    // {
    //     float mouseDeltaX = Input.GetAxis("Mouse Y") * -1; // Flip the direction
    //     tiltAngle += mouseDeltaX * tiltSensitivity;
    //     tiltAngle = Mathf.Clamp(tiltAngle, -maxTiltAngle, maxTiltAngle);
    //     transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, tiltAngle);
    // }
    private void HandleTilting()
    {
        tiltAngle = maxTiltAngle;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, maxTiltAngle);
    }
    private void HandleUnTilt()
    {
        tiltAngle = 0;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
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
                tracker.currentFillLevel += 1;
                
            }
        }
    }

    // Update individual muffin cup's progress slider and fill state
    void UpdateFillProgress(FillTracker tracker, float currentLevel, int numParticles)
    {
        var progressBar = tracker.GetProgressBar();

        // currentLevel / muffinController.GetTotalParticles
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    offset = transform.position - hit.point;
                }
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            isTilting = true;
            HandleTilting();
        }
        else
        {
            isTilting = false;
            HandleUnTilt();
        }

        if (Input.GetMouseButton(0))
        {
            if (isDragging)
            {
                HandleMoving();
            }
        }

        //no longer dragging
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Check tilt 
        if (tiltAngle > pourThreshold)
        {
            if (!pourParticles.isEmitting)
            {
                pourParticles.Play();
            }
            Vector3 rayOrigin = rayPoint.transform.position;
            Vector3 rayDirection = -transform.right;
            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("MuffinTin"))
                {
                    var fillTracker = hit.collider.GetComponent<FillTracker>();
                    if (!fillTracker.isFull)
                    {
                        FillMuffinCup(hit.collider, fillTracker);
                    }
                }
            }
        }
        else
        {
            pourParticles.Stop();
        }
    }
}
