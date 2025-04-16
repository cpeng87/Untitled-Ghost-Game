using UnityEngine;
using UnityEngine.UI;

public class TeapotController : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private bool isTilting = false;
    private float originalZ;
    private float tiltAngle = 0f;

    [Header("Bounds for Dragging")]
    public Vector2 xBound;
    public Vector2 yBound;

    [Header("Tilt Settings")]
    public float tiltSensitivity = 10f;
    public float maxTiltAngle = 40f;

    [Header("Particle System")]
    public ParticleSystem pourParticles;
    public float pourThreshold = 20f;

    [SerializeField] private int teaCounter;
    private int maxTeaParticles = 200;
    private bool outOfTea = false;
    private float totalEmittedParticles = 0;
    public Slider teaProgress;
    [SerializeField] private int neededParticles;
    [SerializeField] ParticleSystem teaSteamParticles;

    void Start()
    {
        originalZ = transform.position.z;

        //set slider values based on neededparticles
        teaProgress.minValue = 0;
        teaProgress.maxValue = neededParticles;
    }

    void Update()
    {

        if (pourParticles.isEmitting)
        {
            float emissionRate = pourParticles.emission.rateOverTime.constant;
            float deltaParticles = emissionRate * Time.deltaTime;
            totalEmittedParticles += deltaParticles;

            if (totalEmittedParticles >= maxTeaParticles)
            {
                outOfTea = true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //clicked on main body collider, begin moving the teapot
                if (hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    offset = transform.position - hit.point;
                }
                //clicked teapot spout, begin tilting the teapot
                else if (hit.collider.CompareTag("Tilt"))
                {
                    isTilting = true;
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

        // Check tilt angle to emit particles or stop emitting particles
        if (tiltAngle > pourThreshold && outOfTea == false)
        {
            if (!pourParticles.isEmitting)
            {
                pourParticles.Play();
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

    //takes in point clicked and moves the teapot to the location dragged to
    private void HandleMoving()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point + offset;

            float clampedX = Mathf.Clamp(targetPosition.x, xBound.x, xBound.y);
            float clampedY = Mathf.Clamp(targetPosition.y, yBound.x, yBound.y);

            transform.position = new Vector3(clampedX, clampedY, originalZ);
        }
    }

    // tilts the teapot based on the mouse's y movement. Limited by max tilt angle.
    private void HandleTilting()
    {
        float mouseDeltaX = Input.GetAxis("Mouse Y") * -1; // Flip the direction
        tiltAngle += mouseDeltaX * tiltSensitivity;
        tiltAngle = Mathf.Clamp(tiltAngle, -maxTiltAngle, maxTiltAngle);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, tiltAngle);
    }

    //when a particle hits one of the trigger objs (teacup), tea counter increases yippee!!
    private void OnParticleTrigger()
    {
        teaCounter += 1;

        //update slider
        teaProgress.value = teaCounter;
        teaSteamParticles.Play();
    }

    //ran when submit button is pressed. Verifies if amount of tea particles caught is greater than the needed to pass.
    //Completes the minigame and passes result to the gameManager.
    public void CheckResults()
    {
        bool result = teaCounter >= neededParticles;
        if (result)
        {
            Debug.Log("success");
        }
        else
        {
            Debug.Log("fail");
        }
        GameManager.Instance.CompleteMinigame(result);
    }
}
