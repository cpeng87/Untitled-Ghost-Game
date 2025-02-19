using UnityEngine;

public class GhostObj : Clickable
{
    [SerializeField] private Ghost scriptable;   //ghost information
    [SerializeField] private Animator idleAnimator;
    [SerializeField] private Animator orderNotificationAnimator; //animator for the notification
    private bool hasTakenOrder;   //flags whether order has been taken or not
    private int seatNum;   //seat number of the current ghost
    private bool isIdle = false; // flag to check if ghost is idle
    private float idleThreshold = 5f; // idle time threshold, can be set to higher value for longer idle time
    private float idleTime = 0f; // accumulates time until idle threshold is reached

    void Start() {
        // Get animator component from ghost prefab
        idleAnimator = GetComponentInChildren<Animator>();
        if (idleAnimator == null || orderNotificationAnimator == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing an Animator component!");
            return;
        }
        if (idleAnimator != null)
        {
            idleAnimator.SetBool("IsFloating", true);
        }
    }
    
    //checks if the ghost order has been taken, if not, takes order when ghost is clicked on
    protected override void OnClicked()
    {
        if (isIdle != true || hasTakenOrder)
        {
            return;
        }
        SetOrderNotification(false);
        hasTakenOrder = GameManager.Instance.orderManager.TakeOrder(scriptable.ghostName, scriptable.order, scriptable.recipesOrdered, seatNum);
    }

    public void SetSeatNum(int newSeatNum)
    {
        if (newSeatNum > 2 || newSeatNum < 0)
        {
            Debug.Log("Invalid seat number");
        }
        seatNum = newSeatNum;
        // SetOrderNotification(true);
    }

    public Ghost GetScriptable()
    {
        return scriptable;
    }

    public void SetHasTakenOrder(bool newStatus)
    {
        hasTakenOrder = newStatus;
    }

    // Condition for checking if the ghost is idle.
    // We can update the condition later (according to game logic)
    private bool IsCustomerIdle()
    {
        return !Input.anyKey;
    }

    void Update() {
        // Update ghost state if it is idle/floating
        if (IsCustomerIdle())
        {
            idleTime += Time.deltaTime;
            if (idleTime >= idleThreshold)
            {
                SetOrderNotification(true);
                isIdle = true;
                // if (idleAnimator != null)
                // {
                //     idleAnimator.SetBool("IsFloating", true);
                // }
            }
        }
        base.Update();
        // else
        // {
        //     idleTime = 0f;
        //     isIdle = false;
        //     if (idleAnimator != null)
        //     {
        //         idleAnimator.SetBool("IsFloating", false);
        //     }
        // }
    }

    private void SetOrderNotification(bool b)
    {
        orderNotificationAnimator.SetBool("isActive", b);
    }
}
