using TMPro;
using UnityEngine;

public class ChefSkipUpdate : MonoBehaviour
{
    [SerializeField] public TMP_Text currencyField;
    [SerializeField] public TMP_Text costField;
    public void PayChef()
    {
        int currency = GameManager.Instance.GetCurrency();
        int orderPrice = GameManager.Instance.orderManager.GetCurrActiveOrderPrice();

        if (currency >= 2 * orderPrice)
        {
            Debug.Log("Chef success");

            GameManager.Instance.CompleteMinigame(true, chefSkip: true);
        }
        else
        {
            Debug.Log("Chef fail");
        }
    }

    public void updateCostField()
    {
        int orderPrice = GameManager.Instance.orderManager.GetCurrActiveOrderPrice();
        costField.text = "Minigame will be skipped. This action will cost TWICE the value of the food/drink being served - this will cost (" + 2 * orderPrice + ").";
        Debug.Log("current cost: " + orderPrice);
    }

    public void updateCurrencyField() 
    {
        int currency = GameManager.Instance.GetCurrency();
        currencyField.text = "Currency: " + currency;
        Debug.Log("current currency: " + currency);
    }
}
