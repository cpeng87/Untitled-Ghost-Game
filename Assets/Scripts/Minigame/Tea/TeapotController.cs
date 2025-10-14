using UnityEngine;
using UnityEngine.UI;

public class TeapotController : MinigameCompletion
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
    [SerializeField] private int maxTeaParticles = 200;
    public bool outOfTea = false;
    private float totalEmittedParticles = 0;
    public Slider teaProgress;
    private int neededParticles;
    private int overflowParticles;
    [SerializeField] ParticleSystem teaSteamParticles;
    private bool isComplete = false;
    private float pourPauseTimer;

    //current gradient is at 65 and 80

    void Start()
    {
        originalZ = transform.position.z;

        //set slider values based on neededparticles
        neededParticles = (int) (0.65f * 200);
        overflowParticles = (int) (0.8f * 200);

        teaProgress.minValue = 0;
        teaProgress.maxValue = 1;
        pourParticles.Stop();
    }

    void Update()
    {
        if (teaCounter > 200 && !isComplete)
        {
            isComplete = true;
            AudioManager.Instance.UnPauseSound();
            AudioManager.Instance.StopSound();
            minigameResult.MinigameResult(false);
        }
        if (!isComplete)
        {
            pourPauseTimer += Time.deltaTime;
            if (pourPauseTimer >= 0.1f)
            {
                AudioManager.Instance.PauseSound();
            }
            if (pourParticles.isEmitting)
            {
                float emissionRate = pourParticles.emission.rateOverTime.constant;
                float deltaParticles = emissionRate * Time.deltaTime;
                totalEmittedParticles += deltaParticles;
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
    }

    //takes in point clicked and moves the teapot to the location dragged to
    private void HandleMoving()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
        mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos) + offset;
        mousePos.z = 0;
        transform.position = mousePos;
    }

    // tilts the teapot based on the mouse's y movement. Limited by max tilt angle.
    private void HandleTilting()
    {
        float mouseDeltaX = Input.GetAxis("Mouse Y"); // Flip the direction
        tiltAngle += mouseDeltaX * tiltSensitivity;
        tiltAngle = Mathf.Clamp(tiltAngle, -maxTiltAngle, maxTiltAngle);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, tiltAngle);
    }

    //when a particle hits one of the trigger objs (teacup), tea counter increases yippee!!
    private void OnParticleTrigger()
    {
        teaCounter += 1;
        AudioManager.Instance.UnPauseSound();
        if (!AudioManager.Instance.CheckPlaying())
        {
            AudioManager.Instance.PlaySound("Pour");
        }

        //update slider
        teaProgress.value = teaCounter * 1.0f / maxTeaParticles;
        teaSteamParticles.Play();
    }

    //ran when submit button is pressed. Verifies if amount of tea particles caught is greater than the needed to pass.
    //Completes the minigame and passes result to the gameManager.
    public void CheckResults()
    {
        bool result = teaCounter >= neededParticles && teaCounter <= overflowParticles;
        if (result && isComplete == false)
        {
            isComplete = true;
            AudioManager.Instance.UnPauseSound();
            AudioManager.Instance.StopSound();
            minigameResult.MinigameResult(result);
        }
        else if (!result && isComplete == false)
        {
            isComplete = true;
            AudioManager.Instance.UnPauseSound();
            AudioManager.Instance.StopSound();
            minigameResult.MinigameResult(result);
        }
    }
}
