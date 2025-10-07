using Manager.CustomerPatience;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.UI;

public enum ParticleState
{
    Flowers,
    Sparkles,
    Scribbes
}

public class GhostObj : Clickable
{
    [SerializeField] private Ghost scriptable;   //ghost information
    [SerializeField] private Animator idleAnimator;
    [SerializeField] private Animator orderNotificationAnimator; //animator for the notification
    [SerializeField] private List<Material> faces = new List<Material>();
    [SerializeField] private SkinnedMeshRenderer face;
    [SerializeField] private ParticleSystem emotionParticles;
    [SerializeField] private List<Material> emotionMaterials = new List<Material>();
    private ParticleState particleState = ParticleState.Flowers;
    private bool hasTakenOrder;   //flags whether order has been taken or not
    private int seatNum;   //seat number of the current ghost
    private bool isIdle = false; // flag to check if ghost is idle
    private float idleThreshold = 0.5f; // idle time threshold, can be set to higher value for longer idle time
    private float idleTime = 0f; // accumulates time until idle threshold is reached
    private bool isSeated;

    void Start() {
        isSeated = false;
        // Get animator component from ghost prefab
        idleAnimator = GetComponentInChildren<Animator>();
        if (idleAnimator == null || orderNotificationAnimator == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing an Animator component!");
            return;
        }
        if (idleAnimator != null)
        {
            // idleAnimator.SetBool("IsFloating", true);
            idleAnimator.Play("IdleFloat", 0, Random.value);
        }
    }

    //checks if the ghost order has been taken, if not, takes order when ghost is clicked on
    protected override void OnClicked()
    {
        if (isIdle != true || hasTakenOrder || GameManager.Instance.state != State.Main)

        {
            return;
        }

        SetOrderNotification(false);
        hasTakenOrder = GameManager.Instance.orderManager.TakeOrder(scriptable.ghostName, scriptable.recipesOrdered, seatNum);
        TicketManager.Instance.SetMakeOrderNotif(true);
    }

    public void SetSeatNum(int newSeatNum)
    {
        if (newSeatNum > 2 || newSeatNum < 0)
        {
            Debug.Log("Invalid seat number");
        }
        seatNum = newSeatNum;
    }

    public Ghost GetScriptable()
    {
        return scriptable;
    }

    public void SetHasTakenOrder(bool newStatus)
    {
        hasTakenOrder = newStatus;
    }

    public void SetSeated(bool seatedStatus)
    {
        isSeated = seatedStatus;
        //Debug.Log("seated " + scriptable.ghostName + "'s ahh : " + isSeated);
    }
    public bool GetSeated()
    {
        //Debug.Log("fetching " + scriptable.ghostName + "'s seat status... : " + isSeated);
        return isSeated;
    }

    // Condition for checking if the ghost is idle.
    // We can update the condition later (according to game logic)
    private bool IsCustomerIdle()
    {
        return !(Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space));
    }

    protected override void Update() {
        if (GetSeated())
        {
            //Debug.Log("seated!! waiting...");
            idleTime += Time.deltaTime;
            if (idleTime >= idleThreshold)
            {
                isIdle = true;
                if (hasTakenOrder == false)
                {
                    SetOrderNotification(true);
                }
            }
        }
        base.Update();
    }

    private void SetOrderNotification(bool b)
    {
        orderNotificationAnimator.SetBool("isActive", b);
    }

    public void EmotionChange(string faceName, string particle = "None")
    {
        if (faceName != "None")
        {
            foreach (Material currFace in faces)
            {
                if (currFace.name == faceName)
                {
                    face.material = currFace;
                    break;
                }
            }
        }
        if (particle == "None")
        {
            emotionParticles.Stop();
        }
        else
        {
            if (particle == "Flowers")
            {
                if (particleState != ParticleState.Flowers)
                {
                    foreach (Material particleType in emotionMaterials)
                    {
                        if (particleType.name == "Flowers")
                        {
                            emotionParticles.GetComponent<ParticleSystemRenderer>().material = particleType;
                        }
                    }
                    particleState = ParticleState.Flowers;
                }
                emotionParticles.Play();
            }
            else if (particle == "Sparkles")
            {
                if (particleState != ParticleState.Sparkles)
                {
                    foreach (Material particleType in emotionMaterials)
                    {
                        if (particleType.name == "Sparkles")
                        {
                            emotionParticles.GetComponent<ParticleSystemRenderer>().material = particleType;
                        }
                    }
                    particleState = ParticleState.Sparkles;
                }
                emotionParticles.Play();
            }
            else if (particle == "Scribbles")
            {
                if (particleState != ParticleState.Scribbes)
                {
                    foreach (Material particleType in emotionMaterials)
                    {
                        if (particleType.name == "Scribbles")
                        {
                            emotionParticles.GetComponent<ParticleSystemRenderer>().material = particleType;
                        }
                    }
                    particleState = ParticleState.Scribbes;
                }
                emotionParticles.Play();
            }
            else
            {
                emotionParticles.Stop();
            }
        }
    }
}
