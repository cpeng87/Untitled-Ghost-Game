using UnityEngine;

public class GhostObj : Clickable
{
    [SerializeField] private Ghost scriptable;
    private bool hasTakenOrder;
    private int seatNum;
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
