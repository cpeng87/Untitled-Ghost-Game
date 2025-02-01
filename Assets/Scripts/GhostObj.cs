using UnityEngine;

public class GhostObj : Clickable
{
    [SerializeField] private Ghost scriptable;   //ghost information
    [SerializeField] private Animator animator; //animator for the game
    private bool hasTakenOrder;   //flags whether order has been taken or not
    private int seatNum;   //seat number of the current ghost
    
    //checks if the ghost order has been taken, if not, takes order when ghost is clicked on
    protected override void OnClicked()
    {
        if (hasTakenOrder)
        {
            return;
        }
        hasTakenOrder = GameManager.Instance.orderManager.TakeOrder(scriptable.ghostName, scriptable.order, scriptable.recipesOrdered, seatNum);
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
}
