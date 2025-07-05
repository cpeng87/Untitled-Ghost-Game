using UnityEngine;

public class ChefSkip : MonoBehaviour
{
    public void PayChef()
    {
        GameManager.Instance.SubtractCurrency(3);
    }
}
