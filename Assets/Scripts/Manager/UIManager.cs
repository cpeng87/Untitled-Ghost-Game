using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TMP_Text currencyField;
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
    {
        TicketManager.Instance.ShowOrders();
    }

    public void UpdateCurrency(int newValue)
    {
        currencyField.text = "Currency: " + newValue;
    }
}
