using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TMP_Text currencyField;
    [SerializeField] private TMP_Text satisfactionField;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void MakeOrderButton()
    { //toggle orders panel visibility
        TicketManager.Instance.ShowOrders();
    }

    public void UpdateCurrency(int newValue)
    {
        currencyField.text = "Currency: " + newValue;
    }

    public void UpdateSatisfaction(int satisfactionLevel)
    {
        if (satisfactionLevel >= 0)
        {
            satisfactionField.text = "Customers are currently satisfied! \n Keep doing a good job!";
        }
        else
        {
            satisfactionField.text = "Customers are not currently satisfied! \n Be careful when making orders!";
        }
    }
}
