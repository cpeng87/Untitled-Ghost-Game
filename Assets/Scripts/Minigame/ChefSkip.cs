using UnityEngine;

public class ChefSkip : MonoBehaviour
{
    public void PayChef()
    {
        int orderPrice = GameManager.Instance.orderManager.GetCurrActiveOrderPrice();
        GameManager.Instance.SubtractCurrency(orderPrice * 2);
    }
}
